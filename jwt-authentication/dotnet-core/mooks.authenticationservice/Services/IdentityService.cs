namespace mooks.authenticationservice.Services
{
    using mooks.authenticationservice.contracts.response;
    using mooks.authenticationservice.Data.Options;
    using mooks.authenticationservice.Domain.Identity;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    public class IdentityService : IIdentityService
    {
        private readonly IStorageService _storageService;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(IStorageService storageService, JwtSettings jwtSettings)
        {
            _storageService = storageService;
            _jwtSettings = jwtSettings;
        }
        public async Task<RegistrationResult> RegisterAsync(User newUser)
        {
            var existingUsers = await _storageService.GetAllUsers();
            var userExists = existingUsers.Where(x => x.Email == newUser.Email).Any();
            if (userExists)
            {
                return new RegistrationResult
                {
                    StatusCode =System.Net.HttpStatusCode.Ambiguous,
                    Success =false,
                    Messages = new[] { $"Uesr with this email {newUser.Email} address is already exists in the system" }
                };
            }
            newUser.Id = Guid.NewGuid().ToString();
            var userAdded = await _storageService.AddUser(newUser);

            var registerUserResponse = new RegistrationResult
            {
                StatusCode = userAdded ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Success = userAdded ? true : false,
                Messages = userAdded ? new[] { $"The uesr {newUser.Email} is created" } : new[] { $"The uesr {newUser.Email} is not created, Internel server error." }
            };

            return registerUserResponse;
        }

        public async Task<LoginResult> LoginAsync(User loginUser)
        {
            var userExistanceCheck = await CheckUserExistance(loginUser);
            if (userExistanceCheck.result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return (LoginResult)userExistanceCheck.result;
            }

            var checkPwdResult = CheckPassword(loginUser, userExistanceCheck.user);
            if (checkPwdResult.StatusCode != System.Net.HttpStatusCode.OK) 
            {
                return (LoginResult)checkPwdResult;
            }

            loginUser.Id = userExistanceCheck.user.Id;
            var loginResult = await GenerateAuthenticationResult(loginUser);

            return loginResult;
        }
       
        public async Task<UpdateUserResult> UpdateUser(User updateUser) 
        {
            var userExistanceCheck = await CheckUserExistance(updateUser);

            if (userExistanceCheck.result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return (UpdateUserResult)userExistanceCheck.result;
            }

            var userUpdated = await _storageService.UpdateUser(updateUser);

            var updateUserResponse = new UpdateUserResult
            {
                StatusCode = userUpdated ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Success = userUpdated ? true : false,
                Messages = userUpdated ? new[] { $"The uesr {updateUser.Email} is updated" } : new[] { $"The uesr {updateUser.Email} is not updated, Internel server error." }
            };

            return updateUserResponse;
        }

        public async Task<UpdateUserResult> ChangePassword(User changePasswordUser) 
        {
            var userExistanceCheck = await CheckUserExistance(changePasswordUser);

            if (userExistanceCheck.result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return (UpdateUserResult)userExistanceCheck.result;
            }

            var passwordUpdated = await _storageService.ChangePassword(changePasswordUser);

            var updatePasswordResponse = new UpdateUserResult
            {
                StatusCode = passwordUpdated ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError,
                Success = passwordUpdated ? true : false,
                Messages = passwordUpdated ? new[] { $"The uesr {changePasswordUser.Email} password is changed successfully" } : new[] { $"The uesr {changePasswordUser.Email} password is not updated, Internel server error." }
            };

            return updatePasswordResponse;
        }

        private async Task<(IResult result, User user)> CheckUserExistance(User user)
        {
            var checkUser = await GetUser(user);

            if (checkUser.exists)
            {
                return (SuccessResult(), checkUser.user);
            }
            else
            {
                return (UserPasswordWrongResult(), null);
            }
        }

        private IResult CheckPassword(User loginUser, User existingUser)
        {
            if(loginUser is null || existingUser is null )
            {
                return UserPasswordWrongResult();
            }

            if (string.Compare(loginUser.Password, existingUser.Password, StringComparison.Ordinal) != 0)
            {
                return UserPasswordWrongResult();
            }

            return SuccessResult();
        }

        private IResult UserPasswordWrongResult()
        {
            return new LoginResult
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Success = false,
                Messages = new[] { "User/Password combination is wrong" }
            };
        }

        private IResult SuccessResult()
        {
            return new LoginResult
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }

        private async Task<(User user, bool exists)> GetUser(User user)
        {
            var existingUsers = await _storageService.GetAllUsers();
            var existingUser = existingUsers.Where(x => x.Email == user.Email);

            return (existingUser.FirstOrDefault(), existingUser.Any());
        }

        private async Task<LoginResult> GenerateAuthenticationResult(User loginUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, loginUser.Email),
                    new Claim("id", loginUser.Id)
            };

            var tokenDescreptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescreptor);

            try
            {
                return new LoginResult
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token),
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Messages = new[] { $"The uesr is authenticated successfully" }
                };
            }
            catch (SecurityTokenEncryptionFailedException ex)
            {
                return new LoginResult
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Messages = new[] { $"error during create token {ex.Message}" }
                };
            }
        }
    }
}
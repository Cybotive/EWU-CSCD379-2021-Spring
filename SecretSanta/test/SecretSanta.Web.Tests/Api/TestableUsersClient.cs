using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Tests.Api
{
    public class TestableUsersClient : IUsersClient
    {
        public Task DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Not utilized by UsersController
        }

        public List<FullUser>? GetAllUsersReturnValue { get; set; } = new();
        public int GetAllAsyncInvocationCount { get; set; }
        public Task<ICollection<FullUser>?> GetAllAsync()
        {
            GetAllAsyncInvocationCount++;
            return Task.FromResult<ICollection<FullUser>?>(GetAllUsersReturnValue);
        }

        public Task<ICollection<FullUser>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Not utilized by UsersController
        }

        public FullUser? GetAsyncFullUser { get; set; }
        public int GetAsyncInvocationCount { get; set; }
        public Task<FullUser?> GetAsync(int id)
        {
            GetAsyncInvocationCount++;

            if(GetAsyncFullUser is not null && id == GetAsyncFullUser.Id)
            {
                return Task.FromResult<FullUser?>(GetAsyncFullUser);
            }

            return null!;
        }

        public Task<FullUser> GetAsync(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Not utilized by UsersController
        }

        public int PostAsyncInvocationCount { get; set; }
        public List<FullUser> PostAsyncInvokedParameters { get; } = new();
        public Task<FullUser> PostAsync(FullUser user)
        {
            PostAsyncInvocationCount++;
            PostAsyncInvokedParameters.Add(user);
            return Task.FromResult(user);
        }

        public Task<FullUser> PostAsync(FullUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Not utilized by UsersController
        }

        public Task PutAsync(int id, UpdateUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task PutAsync(int id, UpdateUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
            //Not utilized by UsersController
        }
    }
}
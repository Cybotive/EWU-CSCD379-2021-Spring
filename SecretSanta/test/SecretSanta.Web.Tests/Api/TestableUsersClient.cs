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
        }

        public List<User>? GetAllUsersReturnValue { get; set; } = new();
        public int GetAllAsyncInvocationCount { get; set; }

        public Task<ICollection<User>?> GetAllAsync()
        {
            GetAllAsyncInvocationCount++;
            return Task.FromResult<ICollection<User>?>(GetAllUsersReturnValue);
        }

        public Task<ICollection<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<FullUser> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<FullUser> GetAsync(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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
        }

        public Task PutAsync(int id, UpdateUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task PutAsync(int id, UpdateUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
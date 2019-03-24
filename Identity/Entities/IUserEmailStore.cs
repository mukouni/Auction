using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Auction.Identity
{
    public interface IUserPhoneNumberStore<TUser> : IUserStore<TUser> where TUser : class
    {
        /// <summary>
        /// Sets the <paramref name="phoneNumber"/> for a <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose phoneNumber should be set.</param>
        /// <param name="phoneNumber">The phoneNumber to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the phoneNumber for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose phoneNumber should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object containing the results of the asynchronous operation, the phoneNumber for the specified <paramref name="user"/>.</returns>
        Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a flag indicating whether the phoneNumber for the specified <paramref name="user"/> has been verified, true if the phoneNumber is verified otherwise
        /// false.
        /// </summary>
        /// <param name="user">The user whose phoneNumber confirmation status should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous operation, a flag indicating whether the phoneNumber for the specified <paramref name="user"/>
        /// has been confirmed or not.
        /// </returns>
        Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the flag indicating whether the specified <paramref name="user"/>'s phoneNumber has been confirmed or not.
        /// </summary>
        /// <param name="user">The user whose phoneNumber confirmation status should be set.</param>
        /// <param name="confirmed">A flag indicating if the phoneNumber has been confirmed, true if the is confirmed otherwise false.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized phoneNumber.
        /// </summary>
        /// <param name="normalizedPhoneNumber">The normalized phoneNumber to return the user for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized phoneNumber.
        /// </returns>
        Task<TUser> FindByPhoneNumberAsync(string normalizedPhoneNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the normalized phoneNumber for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose phoneNumber to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the normalized phoneNumber if any associated with the specified user.
        /// </returns>
        Task<string> GetNormalizedPhoneNumberAsync(TUser user, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the normalized phoneNumber for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose phoneNumber to set.</param>
        /// <param name="normalizedPhoneNumber">The normalized phoneNumber to set for the specified <paramref name="user"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SetNormalizedPhoneNumberAsync(TUser user, string normalizedPhoneNumber, CancellationToken cancellationToken);
    }
}
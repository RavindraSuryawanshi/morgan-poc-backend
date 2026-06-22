using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morgan.SalesforceSyncPOC.Application.DTOs.Requests;
using Morgan.SalesforceSyncPOC.Core.Contracts;
using Morgan.SalesforceSyncPOC.Core.DataModels;
using Morgan.SalesforceSyncPOC.Core.Enums;
using Morgan.SalesforceSyncPOC.Core.Events;
using System.Text.Json;

namespace Morgan.SalesforceSyncPOC.WebApi.Controllers
{
    /// <summary>
    /// Manages user CRUD operations and creates corresponding outbox events
    /// for Salesforce synchronization.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IRepository<OutboxMessage> _outboxMessageRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IRepository<User> userRepository, IRepository<OutboxMessage> outboxMessageRepository,
            ILogger<UsersController> logger)
        {
            _usersRepository = userRepository;
            _outboxMessageRepository = outboxMessageRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user and publishes a UserCreated outbox event.
        /// </summary>
        [HttpPost]
        public IActionResult Create(CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid();

            var user = new User
            {
                ExternalId = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };

            
            var insertedUser = _usersRepository.Add(user);
            _usersRepository.SaveChanges();
            _logger.LogInformation($"User Created {insertedUser.Id}");

            var outbox = new OutboxMessage
            {
                CorrelationId = correlationId,
                EventType = UserEvents.Created,
                Status = OutboxStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                Payload = JsonSerializer.Serialize(
                    new
                    {
                        UserId = user.Id
                    })
            };

            _outboxMessageRepository.Add(outbox);
            _outboxMessageRepository.SaveChanges();
            _logger.LogInformation("Outbox message created. EventId: {EventId}, EventType: {EventType}, AggregateId: {AggregateId}",
                outbox.Id,
                outbox.EventType,
                user.Id);

            return Ok(user.Id);
        }


        /// <summary>
        /// Retrieves all users.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _usersRepository.GetAll().ToList();

            _logger.LogInformation("Retrieved {Count} users", users.Count);

            return Ok(users);
        }


        /// <summary>
        /// Retrieves a user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _usersRepository.Get(id);

            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                return NotFound();
            }

            _logger.LogInformation("Retrieved user. UserId: {UserId}", id);

            return Ok(user);
        }

        /// <summary>
        /// Updates an existing user and publishes a UserUpdated outbox event.
        /// </summary>
        /// <param name="id">User identifier.</param>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateUserRequest request)
        {
            var user = _usersRepository.Get(id);

            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                return NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Phone = request.Phone;

            _usersRepository.Update(user);
            _usersRepository.SaveChanges();

            _logger.LogInformation("User updated. UserId: {UserId}", id);

            var outbox = new OutboxMessage
            {
                CorrelationId = Guid.NewGuid(),
                EventType = UserEvents.Updated,
                Status = OutboxStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                Payload = JsonSerializer.Serialize(new
                {
                    UserId = user.Id
                })
            };

            _outboxMessageRepository.Add(outbox);
            _outboxMessageRepository.SaveChanges();

            _logger.LogInformation(
                "Outbox message created. EventId: {EventId}, EventType: {EventType}, AggregateId: {AggregateId}",
                outbox.Id,
                outbox.EventType,
                user.Id);

            return NoContent();
        }

        /// <summary>
        /// Deletes a user and publishes a UserDeleted outbox event.
        /// </summary>
        /// <param name="id">User identifier.</param>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _usersRepository.Get(id);

            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                return NotFound();
            }

            _usersRepository.Delete(user.Id);
            _usersRepository.SaveChanges();

            _logger.LogInformation("User deleted. UserId: {UserId}", id);

            var outbox = new OutboxMessage
            {
                CorrelationId = Guid.NewGuid(),
                EventType = UserEvents.Deleted,
                Status = OutboxStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                Payload = JsonSerializer.Serialize(new
                {
                    UserId = id
                })
            };

            _outboxMessageRepository.Add(outbox);
            _outboxMessageRepository.SaveChanges();

            _logger.LogInformation(
                "Outbox message created. EventId: {EventId}, EventType: {EventType}, AggregateId: {AggregateId}",
                outbox.Id,
                outbox.EventType,
                id);

            return NoContent();
        }
    }
}


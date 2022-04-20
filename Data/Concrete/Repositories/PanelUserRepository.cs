using Core.Models;
using Core.Extensions;
using Data.Abstract;
using Data.Concrete.Contexts;
using Entities;
using Microsoft.EntityFrameworkCore;
using static Core.DTOs.User;
using static Core.Constants.UserConstants;
using Core.Utilities;

namespace Data.Concrete.Repositories
{
    public class PanelUserRepository : IPanelUserRepository
    {
        private readonly BottleTrackingDbContext _dbContext;

        public PanelUserRepository(BottleTrackingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PanelUserAddRequest data)
        {
            HashingHelper.CreatePasswordHash(data.Password, out string hash, out string salt);
            var user = new PanelUser
            {
                Name = data.Name,
                Surname = data.Surname,
                Email = data.Email,
                Type = (int)data.UserType,
                Password = hash,
                PasswordSalt = salt
            };

            _dbContext.PanelUsers.Add(user);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _dbContext.PanelUsers.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.Deleted = true;
                _dbContext.SaveChanges();
            }
        }

        public PagedData<PanelUserGetResponse> GetAll(RequestFilter filter)
        {
            return _dbContext.PanelUsers.AsNoTracking()
                                        .Select(x => new PanelUserGetResponse
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Surname = x.Surname,
                                            Email = x.Email,
                                            UserType = (Types)x.Type,
                                            CreatedDate = x.CreateDate
                                        })
                                        .Filter(ref filter)
                                        .OrderBy(filter.Order.Field, filter.Order.IsDesc)
                                        .Paginate(filter.PageNumber, filter.PageSize);
        }

        public PanelUser GetByEmail(string email)
        {
            return _dbContext.PanelUsers.Where(x => x.Email == email).FirstOrDefault();
        }

        public PanelUser GetById(int id)
        {
            return _dbContext.PanelUsers.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<KioskUserGetResponse> GetUnassignedKioskUsers()
        {
            return _dbContext.PanelUsers.Where(x => x.Type == (int)Types.Kiosk &&
                                                    !_dbContext.Stations.Any(s => s.PanelUserId == x.Id))
                                        .Select(x => new KioskUserGetResponse
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Surname = x.Surname,
                                        }).ToList();
        }

        public int GetUserStationIdByUserId(int userId)
        {
            return _dbContext.Stations.Where(x => x.PanelUserId == userId).Select(x => x.Id).FirstOrDefault();
        }

        public void ResetPassword(PanelUser user, string password)
        {
            
                HashingHelper.CreatePasswordHash(password, out string hash, out string salt);
                user.Password = hash;
                user.PasswordSalt = salt;
        }

        public void Update(PanelUserUpdateRequest data, PanelUser user)
        {
            if (user == null)
            {
                user = _dbContext.PanelUsers.Where(x => x.Id == data.Id).FirstOrDefault();
            }

            user.Name = data.Name;
            user.Surname = data.Surname;
            user.Email = data.Email;
            user.Type = (int)data.UserType;
            _dbContext.SaveChanges();
        }
    }
}

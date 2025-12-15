using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Services.Abstractions;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BackgroundServices:BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BackgroundServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var expiredSubscriptions = await _unitOfWork.GetRepository<UserGroup>()
                    .GetAllAsyncs(ug=>ug.IsActive==true && ug.ExpirationDate<=DateTime.UtcNow && ug.RoleInGroup==Role.Student);
                if (expiredSubscriptions.Any())
                {
                    foreach (var subscription in expiredSubscriptions)
                    {
                        subscription.IsActive = false;
                    }

                    _unitOfWork.GetRepository<UserGroup>().UpdateRange(expiredSubscriptions);
                    await _unitOfWork.SaveChanges();
                }
                await Task.Delay(TimeSpan.FromMinutes(40), stoppingToken);
            }
        }
    }
}

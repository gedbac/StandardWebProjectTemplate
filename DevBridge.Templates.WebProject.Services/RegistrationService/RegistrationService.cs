﻿using System;
using System.Linq;
using DevBridge.Templates.WebProject.DataContracts;
using DevBridge.Templates.WebProject.DataEntities.Entities;
using DevBridge.Templates.WebProject.DataEntities.Enums;
using DevBridge.Templates.WebProject.ServiceContracts;

using NHibernate;

namespace DevBridge.Templates.WebProject.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IAgreementManagementService agreementManagementService;
        private readonly IUnitOfWork unitOfWork;

        public RegistrationService(IUnitOfWork unitOfWork, IAgreementManagementService agreementManagementService)
        {
            this.agreementManagementService = agreementManagementService;
            this.unitOfWork = unitOfWork;
        }

        public void RegisterCustomer(string name)
        {
            RegisterCustomer(name, CustomerType.Standard);
        }

        public void RegisterCustomer(string name, CustomerType customerType)
        {            
            try
            {                                
                Customer customer = new Customer();
                customer.Name = name;
                customer.Type  = customerType;
                customer.CreatedOn = DateTime.Now;
                customer.Agreements.Add(new Agreement
                                            {
                                                Number = agreementManagementService.GenerateAgreementNumber(),
                                                CreatedOn =  DateTime.Now,
                                                Customer = customer
                                            });
                unitOfWork.Session.Save(customer);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {                
                throw new RegistrationException(string.Format("Failed to register customer with name '{0}' and type {1}.", name, customerType), ex);
            }                                
        }

        public void UnregisterCustomer(int id)
        {
            try
            {
                var customer = unitOfWork.Session.Load<Customer>(id, LockMode.None);
                unitOfWork.Session.Delete(customer);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new RegistrationException(string.Format("Failed to unregister customer {0}.", id), ex);
            }
        }
    }
}

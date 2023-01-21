using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class UserEntity: AuditableBaseEntity
{
    public UserEntity()
    {
    }

    public UserEntity(
        string email,
        string password,
        string role,
        string phone,
        string lastName,
        string firstName,
        string middleName
    )
    {
        Email = email;
        Password = password;
        Role = role;
        Phone = phone;
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Phone { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
}
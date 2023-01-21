using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers;

public class MessageResponse
{
    public MessageResponse(string message)
    {
        Message = message;
    }

    public string Message { get; set; } = string.Empty;
}
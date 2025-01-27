﻿using System.Net;

namespace ToDoListService.Framework.Exceptions;

public class BaseRestException: Exception
{
    public string ErrorMessage { get; private set; }

    public int ErrorCode { get; private set; }

    public BaseRestException(string errorMessage, HttpStatusCode errorCode) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorCode = (int) errorCode;
    }
}

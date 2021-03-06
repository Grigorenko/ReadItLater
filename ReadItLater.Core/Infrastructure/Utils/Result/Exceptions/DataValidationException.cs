using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using FluentValidation.Results;

namespace ReadItLater.Core.Infrastructure
{

    [Serializable]
    public class DataValidationException : Exception, ICustomResultException
    {
        public IEnumerable<IResultError> Errors { get; private set; }

        public DataValidationException(ICollection<ValidationFailure> errors)
        {
            Errors = errors.Select(e => new DefaultResultError(e.PropertyName, e.ErrorMessage));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue(nameof(Errors), Errors, typeof(IEnumerable<IResultError>));

            base.GetObjectData(info, context);
        }
    }
}

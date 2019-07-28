using System;
using System.Net.Mail;

namespace RabbitMQ.Core.Entities
{
    public class MailSendResult
    {

        public MailMessage OriginalMessage { get; set; }
        public bool BooleanResult { get; set; }
        public Exception Exception { get; set; }
        private string _message = string.Empty;
        public string ResultMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_message) == false)
                {
                    return _message;
                }
                else if (Exception != null)
                {
                    return Exception.Message;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                _message = value;
            }
        }
       

        public MailSendResult(MailMessage originalMessage, bool result, string message = "")
        {
            OriginalMessage = originalMessage ?? throw new ArgumentNullException(nameof(originalMessage));
            BooleanResult = result;
            ResultMessage = message;
        }

        public MailSendResult(MailMessage originalMessage, Exception exception)
        {
            OriginalMessage = originalMessage ?? throw new ArgumentNullException(nameof(originalMessage));
            BooleanResult = false;
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

    }
}
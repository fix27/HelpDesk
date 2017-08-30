using HelpDesk.Entity;
using System;
using HelpDesk.Common.Helpers;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Пользователь исполнитель/диспетчер
    /// </summary>
    public class WorkerUserDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string UserName
        {
            get
            {
                if(Worker == null)
                    return String.Format("{0} ({1})", Name, UserType.Name);

                return String.Format("{0} ({1} - {2})", Name, UserType.Name, Worker.Name);

            }
        }

        /// <summary>
        /// E-mail (в качестве логина используется, может также только имя почтового ящика без домена)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип пользователя
        /// </summary>
        public TypeWorkerUser UserType { get; set; }

        /// <summary>
        /// Исполнитель (null для Диспетчера)
        /// </summary>
        public Worker Worker { get; set; }

    }
}

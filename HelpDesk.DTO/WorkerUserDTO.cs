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
                switch (UserType)
                {
                    case TypeWorkerUserEnum.Dispatcher:
                        return String.Format("{0} ({1})", 
                            Name, TypeWorkerUserEnum.Dispatcher.GetDisplayName());

                    case TypeWorkerUserEnum.Worker:
                        return String.Format("{0} ({1})", 
                            Name, Worker.Name);

                    case TypeWorkerUserEnum.WorkerAndDispatcher:
                        return String.Format("{0} ({1} - {2})", 
                            Name, Worker.Name, TypeWorkerUserEnum.WorkerAndDispatcher.GetDisplayName());
                }

                return null;
                
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
        public TypeWorkerUserEnum UserType { get; set; }

        /// <summary>
        /// Исполнитель (null для Диспетчера)
        /// </summary>
        public Worker Worker { get; set; }

    }
}

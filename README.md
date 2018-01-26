# HelpDesk - система для подачи, диспетчеризации и трекинга заявок на ТО ВТ/КМТ и сопровождение ПО
<img src=https://github.com/knyazkov-ma/HelpDesk/blob/master/HelpDesc.png>
<p>Порядок разворачивания (VS 2015, MS SQL SERVER 2012, RabbitMQ)</p>
<ul>
  <li>Создать БД HelpDesk</li>
  <li>Изменить коннекшнстринг в NHibernate.cfg.xml (проект Data/Implementation/HelpDesk.Data.NHibernate)</li>
  <li>Создать в RabbitMQ очередь HelpDesk (порядок установки RabbitMQ см. в rabbitmq.txt)</li>
  <li>Redis для Windows скачать тут https://github.com/rgl/redis/downloads (параметры кэша в Web.config)</li>
</ul>

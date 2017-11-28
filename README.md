# HelpDesk - система для подачи, диспетчеризации и трекинга заявок на ТО ВТ/КМТ и сопровождение ПО
<img src=https://user-images.githubusercontent.com/15001513/33117479-6fa13814-cf8a-11e7-805f-9cbc3c132395.png>
<p>Порядок разворачивания (VS 2015, MS SQL SERVER 2012, RabbitMQ)</p>
<ul>
  <li>Создать БД HelpDesk</li>
  <li>Изменить коннекшнстринг в NHibernate.cfg.xml (проект Data/Implementation/HelpDesk.Data.NHibernate)</li>
  <li>Создать в RabbitMQ очередь HelpDesk (порядок установки RabbitMQ см. в rabbitmq.txt)</li>
</ul>

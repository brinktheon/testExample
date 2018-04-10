use Warehouse;
/*Задание 8*/
select m.First_Name as 'Имя', m.Last_Name 'Фамилия'
from Managers m
inner join  Warehouse_Manager wm 
		on wm.Manager_Id = m.id
inner join Warehouse_Store ws 
		on ws.Warehouse_Id = wm.Warehouse_Id
GROUP BY m.First_Name, m.Last_Name
HAVING COUNT(ws.Warehouse_Id) > 1;

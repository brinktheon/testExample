use Warehouse;
/*Задание 8*/
select m.First_Name as 'Имя', m.Last_Name 'Фамилия'
from Managers m 
inner join (	
				select wm.Manager_Id
				from Warehouse_Manager wm 					
				inner join Warehouse_Store ws 
						on ws.Warehouse_Id = wm.Warehouse_Id
				Group by wm.Manager_Id
				Having  Count(ws.Store_Id) > 1

			) as inj on m.id = inj.Manager_Id;
				
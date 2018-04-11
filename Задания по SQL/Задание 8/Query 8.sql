use Warehouse;
/*Задание 8*/
select m.First_Name as 'Имя', m.Last_Name 'Фамилия'
from Managers m 		
	Where 1 < (	
				select Count(ws.Store_Id)
				from Warehouse_Store ws 					
				inner join Warehouse_Manager wm 
						on ws.Warehouse_Id = wm.Warehouse_Id AND
						wm.Manager_Id = m.id
			)

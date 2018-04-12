use Warehouse;
/*Задание 9*/
select m.First_Name as 'Имя', m.Last_Name as 'Фамилия'
from  Managers m
inner join
			(
			select top 3 wm.Manager_Id
					from Warehouse_Manager wm
					inner join Warehouse_Products wp 
						on wp.Warehouse_Id = wm.Warehouse_Id
			Group by wm.Manager_Id
			Order by Sum(wp.Quantity_Product) Desc
			) as inj on inj.Manager_Id = m.id;

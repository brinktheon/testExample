use Warehouse;
/*Задание 9*/
select top 3 m.First_Name as 'Имя', m.Last_Name as 'Фамилия',
			 (
					select Sum(wp.Quantity_Product) as 'Кол-во товара на складах'
					from Warehouse_Manager wm
					inner join Warehouse_Products wp 
						on wp.Warehouse_Id = wm.Warehouse_Id AND wm.Manager_Id = m.id
			  )
from Managers m;
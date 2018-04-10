use Warehouse;
/*Задание 10*/
UPDATE Warehouse_Products 
SET Quantity_Product = Quantity_Product + 1

from Products p
		inner join Warehouse_Products wp
			on wp.Product_Id = p.id
		inner join Warehouse_Store ws
			on ws.Warehouse_Id = wp.Warehouse_Id
		inner join Stores s
		on s.id = ws.Store_Id

WHERE 
			p.Name Like '%Фисташка%' AND
			s.Name Like 'Мир пустоты';
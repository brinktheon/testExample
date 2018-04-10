use Warehouse;
/*Задание 3*/
select p.Name as 'Наименование продукта', pt.Type as 'Тип товара', ps.Name as 'Производитель' 
from Products p
INNER join Products_Types pt
	on p.Type_Product_Id = pt.id
INNER join Producers ps
	on p.Producer_Id = ps.id
where p.Price > 50;

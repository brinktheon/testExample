use Warehouse;
/*Задание 5*/
select pt.Type as 'Тип товара', p.Price as 'Минимальная Цена' 
from Products p
INNER join Products_Types pt
	on p.Type_Product_Id = pt.id
Inner join (
			Select pt.Type, Min(p.Price) minPrice
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
			) inn on pt.Type = inn.Type AND
					 p.Price = inn.minPrice;
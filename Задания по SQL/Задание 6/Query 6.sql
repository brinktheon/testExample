use Warehouse;
/*Задание 6*/
select pt.Type as 'Тип товара', p.Price as 'Минимальная Цена'
from Products p
INNER join Products_Types pt
	on p.Type_Product_Id = pt.id
inner join (
			Select pt.Type, SUM(p.Price) SumPrice
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
			) j1 on j1.Type = pt.Type AND
				SumPrice > 1000

Inner join (
			Select pt.Type, Min(p.Price) minPrice
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
			) inn on pt.Type = inn.Type AND
					 p.Price = inn.minPrice;
				
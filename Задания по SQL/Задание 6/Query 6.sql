use Warehouse;
/*Задание 6*/	
Select pt.Type as 'Тип товара',  Min(p.Price) as 'Минимальная Цена'
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
			Having sum(p.Price) > 1000
			
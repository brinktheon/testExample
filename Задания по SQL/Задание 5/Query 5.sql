use Warehouse;
/*Задание 5*/
Select pt.Type as 'Тип продукта', Min(p.Price) 'Минимальная цена'
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
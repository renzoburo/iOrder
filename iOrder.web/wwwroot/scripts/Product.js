function OrderViewModel(orderData)
{
    if (!orderData)
        return undefined;
    var detail = {
        id: ko.observable(orderData.id),
        clientId: ko.observable(orderData.clientId),
        dateCreated: ko.observable(orderData.dateCreated),
        dateUpdated: ko.observable(orderData.dateUpdated)
    };
    return detail;
}
function ProductOrderViewModel(productOrderData)
{
    if (!productOrderData)
        return undefined;
    var details = {
        id: ko.observable(productOrderData.id),
        orderId: ko.observable(productOrderData.orderId),
        productId: ko.observable(productOrderData.productId),
        quantity: ko.observable(productOrderData.quantity)
    };
    return details;
}
function ProductViewModel(productData)
{
    if (!productData)
        return undefined;
    var details = {
        id: ko.observable(productData.id),
        title: ko.observable(productData.title),
        description: ko.observable(productData.description),
        price: ko.observable(productData.price),
        image: ko.observable(productData.image),
        formattedPrice: ko.computed(function () {
            var intPrice = parseInt(productData.price);
            return intPrice ? intPrice.toFixed(2) + " ZAR" : "0.00 ZAR";
        }),
        canDelete: ko.observable(false)
    };

    return details;
}
function BasketViewModel(orderInfoData, orderProductsData)
{
    if (!orderInfoData && !orderProductsData)
        return undefined;
    var details = {
        orderInfo: ko.observable(new OrderViewModel(orderInfoData)),
        orderProducts: ko.observableArray(orderProductsData)
    };
    return details;
}
function DeleteItemViewModel(basketData, productData)
{
    if (!basketData && !productData)
        return undefined;
    var details = {
        basket: ko.observable(basketData),
        product: ko.observable(productData)
    };
    return details;
}
function ProductListViewModel()
{
    var self = this;
    self.clientId = ko.observable();
    self.products = ko.observableArray();
    self.basket = ko.observable();
    self.totalItems = ko.computed(function()
    {
        var itemCount = 0;

        if (self.basket()) {
            if (self.basket().orderProducts)
            {
                for (var i = 0; i < self.basket().orderProducts().length; i++) {
                    var quantity = self.basket().orderProducts()[i].quantity();
                    itemCount += parseInt(quantity);
                }
            }
        }
        return itemCount;
    }, self);
    self.totalPrice = ko.computed(function()
    {
        var totalPrice = 0;

        if (self.basket())
        {
            if (self.basket().orderProducts) {
                for (var i = 0; i < self.basket().orderProducts().length; i++) {
                    var quantity = self.basket().orderProducts()[i].quantity();
                    var itemId = self.basket().orderProducts()[i].productId();
                    var price = self.getProductPrice(itemId);
                    var itemPrice = (quantity * price);
                    totalPrice = totalPrice + itemPrice;
                }
            }
        }

        return totalPrice.toFixed(2) + " ZAR";
    }, self);
    self.checkoutEnabled = ko.computed(function()
    {
        return self.totalItems() >= 1;
    }, self);
    self.getProductPrice = function (itemId)
    {
        if (self.products())
        {
            for (var i = 0; i < self.products().length; i++)
            {
                if (itemId === self.products()[i].id())
                {
                    return self.products()[i].price();
                }
            }
        }
        return 0;
    };
    self.getProducts = function ()
    {
        $.when(
            $.ajax({
                url: "api/GetClientBasket",
                type: "GET",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data)
                    {
                        var orderProducts = ko.observableArray();
                        for (var i = 0; i < data.OrderProducts.length; i++) {
                            var productOrder = new ProductOrderViewModel(data.OrderProducts[i]);
                            orderProducts.push(productOrder);
                        }
                        var basketData = new BasketViewModel(data.OrderInfo, orderProducts());
                        self.basket(basketData);
                        for (var j = 0; j < self.products().length; j++)
                        {
                            for (var k = 0; k < self.basket().orderProducts().length; k++)
                            {
                                if (self.products()[j].id() === self.basket().orderProducts()[k].productId())
                                {
                                    self.products()[j].canDelete(true);
                                }
                            }
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        ).then(
            $.ajax({
                url: "api/GetProducts",
                type: "GET",
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data)
                    {
                        for (var i = 0; i < data.length; i++) {
                            var product = new ProductViewModel(data[i]);
                            self.products.push(product);
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        );
    };
    self.addProductToBasket = function (productData)
    {
        var product = {
            id: productData.id,
            title: productData.title,
            description: productData.description,
            price: productData.price,
            image: productData.image
        };

        $.ajax({
            url: "api/AddToBasket",
            type: "POST",
            dataType: "json",
            data: ko.toJSON(product),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data)
                {
                    var orderProducts = ko.observableArray();
                    for (var i = 0; i < data.OrderProducts.length; i++)
                    {
                        var productOrder = new ProductOrderViewModel(data.OrderProducts[i]);
                        orderProducts.push(productOrder);
                    }
                    self.basket(new BasketViewModel(data.OrderInfo, orderProducts()));
                    productData.canDelete(true);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(ko.toJSON(jqXHR));
            }
        });
    };
    self.removeProductFromBasket = function (productData)
    {
        var product = {
            id: productData.id,
            title: productData.title,
            description: productData.description,
            price: productData.price,
            image: productData.image
        };

        var deleteParam = new DeleteItemViewModel(self.basket, product);

        $.ajax({
            url: "api/RemoveFromBasket",
            type: "DELETE",
            dataType: "json",
            data: ko.toJSON(deleteParam),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data)
                {
                    var orderProducts = ko.observableArray();
                    if (data.OrderProducts !== null)
                    {
                        for (var i = 0; i < data.OrderProducts.length; i++) {
                            var productOrder = new ProductOrderViewModel(data.OrderProducts[i]);
                            orderProducts.push(productOrder);
                        }
                    }
                    self.basket(new BasketViewModel(data.OrderInfo, orderProducts()));
                    productData.canDelete(false);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
    self.checkout = function()
    {
        $.ajax({
            url: "api/Checkout",
            type: "POST",
            dataType: "json",
            data: ko.toJSON(self.basket()),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data)
                {
                    if (self.basket())
                    {
                        self.basket(new BasketViewModel(undefined, undefined));
                        alert("The order has been placed and the item(s) will be delivered to your desk.");
                        if (self.products() && self.products().length > 0)
                        {
                            for (var i = 0; i < self.products().length; i++) {
                                self.products()[i].canDelete(false);
                            }
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
}

$(document).ready(function () {
    try {
        var productListViewModel = new ProductListViewModel();
        ko.applyBindings(productListViewModel);
        productListViewModel.getProducts();
//        $(".shopping-cart").sticky({topSpacing:50});
    }
    catch (e) {
        alert(e);
    }
});

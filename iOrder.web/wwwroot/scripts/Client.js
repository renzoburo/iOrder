function AddressTypeViewModel(addressTypeData) {
    var detail = {
        id: ko.observable(addressTypeData.id),
        addressTypeName: ko.observable(addressTypeData.addressTypeName)
    };
    return detail;
}
function getProductRedirectUrl(clientData)
{
    return "Product/Product/" + clientData.id();
}
function ClientViewModel(clientData) {
    var detail = {
        id: ko.observable(clientData.id),
        firstNames: ko.observable(clientData.firstNames),
        surname: ko.observable(clientData.surname),
        addressType: ko.observable(clientData.addressType),
        streetAddress: ko.observable(clientData.streetAddress),
        suburb: ko.observable(clientData.suburb),
        city: ko.observable(clientData.city),
        postalCode: ko.observable(clientData.postalCode)
    };
    return detail;
}
function ClientListViewModel()
{
    var self = this;
    self.clients = ko.observableArray();
    self.addressTypes = ko.observableArray();
    self.addressTypesLoaded = false;
    self.valueChanged = ko.observable(false);
    self.onValueChanged = function(data)
    {
        if (!self.valueChanged())
        {
            self.valueChanged(true);
        }
    };

    self.goOrder = function (client)
    {
        var url = getProductRedirectUrl(client);
        window.location.href = url;
    };
    self.addClient = function()
    {
        var newClient = {
            id: "",
            firstName: "",
            surname: "",
            addressType: "",
            streetAddress: "",
            suburb: "",
            city: "",
            postalCode: ""
        };
        self.clients.push(new ClientViewModel(newClient));
    };
    self.saveClient = function (clientData) {

        var client = {
            Id: clientData.id(),
            FirstNames: clientData.firstNames(),
            Surname: clientData.surname(),
            AddressType: clientData.addressType(),
            StreetAddress: clientData.streetAddress(),
            Suburb: clientData.suburb(),
            City: clientData.city(),
            PostalCode: clientData.postalCode()
        };

        $.ajax({
            url: "api/SaveClient",
            type: "POST",
            dataType: "json",
            data: JSON.stringify(client),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data)
                {
                    clientData.id(data.id);
                    self.valueChanged(false);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
    self.removeClient = function (clientData) {

        var client = {
            Id: clientData.id(),
            FirstNames: clientData.firstNames(),
            Surname: clientData.surname(),
            AddressType: clientData.addressType(),
            StreetAddress: clientData.streetAddress(),
            Suburb: clientData.suburb(),
            City: clientData.city(),
            PostalCode: clientData.postalCode()
        };

        $.ajax({
            url: "api/DeleteClient",
            type: "DELETE",
            dataType: "json",
            data: JSON.stringify(client),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data)
                {
                    self.clients.remove(clientData);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
    self.getClients = function()
    {
        $.when(
            $.ajax({
                url: "api/GetAddressTypes",
                type: "GET",
                dataType: "json",
                data: "{}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data) {
                        self.addressTypes([]);
                        for (var i = 0; i < data.length; i++) {
                            var addressType = new AddressTypeViewModel(data[i]);
                            self.addressTypes.push(addressType);
                        }
                        self.addressTypesLoaded = true;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        ).then(
            $.ajax({
                url: "api/GetClients",
                type: "GET",
                dataType: "json",
                data: "{}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data) {
                        for (var i = 0; i < data.length; i++) {
                            var client = new ClientViewModel(data[i]);
                            self.clients.push(client);
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        );
    };
}

$(document).ready(function ()
{
    try
    {
        var clientListViewModel = new ClientListViewModel();
        ko.applyBindings(clientListViewModel);
        clientListViewModel.getClients();
    }
    catch (e)
    {
        alert(e);
    } 
});

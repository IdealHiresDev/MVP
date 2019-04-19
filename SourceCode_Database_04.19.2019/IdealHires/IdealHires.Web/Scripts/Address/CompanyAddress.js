//Start "RELATED ENTITIES" details and removed
$('.removeRelatedEntities').unbind().click(function () {
    var sno = $(this).parent().parent().find("td.Sno").first().text();
    var row = $(this).parent().parent();
    DeleteConfirmation2('btnWorkExpDelete', true)

    $('#btnWorkExpDelete').click(function () {
        var a = $("#btnWorkExpDelete").attr("data-val");
        if (a=='true') {
            
            var $ItemSno = $("#itemTableRelatedEntities tbody").find("td.Sno");
            if ($ItemSno.length != 0) {
                $ItemSno.each(function () {
                    if ($(this).text() > sno) {
                        var NewSno = Number($(this).text()) - 1;
                        $(this).text(NewSno);
                    }
                });
            }
        }
        
        row.remove();
        restoreControlsId('#itemTableRelatedEntities');
        checktableempty();
        $("#CommonDeleteModel").modal('hide');
        return false;
    });
});

function restoreControlsId(table) {
    var i = 0;
    var newIndex = 0;
    $(table).find('tbody tr').not('td:last').each(function (i, el) {
        newIndex = i;
        for (var j = 0; j < el.children.length - 1; j++) {
            if (undefined != $((el.children[j].children[0])).attr('id')) {
                var id = $((el.children[j].children[0])).attr('id').split('_');
                id[1] = $((el.children[j].children[0])).attr('id').split('_')[1].replace(id[1], newIndex)
                id = id[0] + "_" + id[1] + "__" + id[3];
                $((el.children[j].children[0])).attr("id", id);
                var name = $((el.children[j].children[0])).attr("name").split('[');
                var index = $((el.children[j].children[0])).attr("name").split('[')[1].split(']')[0].replace(name[1].split(']')[0], newIndex)
                name = name[0] + "[" + index + "]" + name[1].split(']')[1];
                $((el.children[j].children[0])).attr("name", name);
            }
        }
    });
}

function checktableempty() {
    var rowCount = $("#itemTableRelatedEntities > tbody > tr").length;
    if (rowCount < 1) {
        $("#emptymsg").html('<td colspan="7" class="errormsg text-center" style="font-size:17px;border: 1px #ccc solid;">No item added yet</td>');
    }
    else {
        $("#emptymsg").html("");
    }
}
function isNumber(evt) {
    debugger;
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        $("#error-form").append('<tr><td colspan="5"  style="color:#d9534f;">' + 'ZipCode must be numeric</td></tr>');
        return false;
       
    }
    $("#error-form").hide();
    return true;
}


function addMoreAddress() {
    var index = $('#itemTableRelatedEntities tbody tr').length;
    if (index == 0 || validateRelatedEntities(index)) {        
        var addressType = '<select class="form-control ng-untouched ng-pristine ng-invalid IsDecimal" maxlength="100" id="companyAddressDTOList_' + index + '__AddressTypeId" name="companyAddressDTOList[' + index + '].AddressTypeId"> </select>';
        var country = '<select class="form-control ng-untouched ng-pristine ng-invalid IsDecimal" maxlength="100" id="companyAddressDTOList_' + index + '__CountryId" name="companyAddressDTOList[' + index + '].CountryId" > </select>';
        var address = '<input class="zip-input form-control ng-untouched ng-pristine ng-invalid"  autocomplete = "off" visible=true; maxlength="100"  placeholder="Street Address"  id = "companyAddressDTOList_' + index + '__Address" name= "companyAddressDTOList[' + index + '].Address">';
        var state = '<select class="form-control ng-untouched ng-pristine ng-invalid IsDecimal" maxlength="100" id="companyAddressDTOList_' + index + '__StateId" name="companyAddressDTOList[' + index + '].StateId" > </select>';
        var city = '<input class="city-input form-control ng-untouched ng-pristine ng-invalid" autocomplete = "off" visible=true; maxlength="100"  placeholder="City" id="companyAddressDTOList_' + index + '__City" name="companyAddressDTOList[' + index + '].City"> ';
        var zipcode = '<input  class="zip-input form-control ng-untouched ng-pristine ng-invalid" onkeypress="return isNumber(event)" autocomplete = "off" visible=true; maxlength="100"  placeholder="Zip Code"  id = "companyAddressDTOList_' + index + '__ZipCode" name= "companyAddressDTOList[' + index + '].ZipCode">';
        var action = '<a class="remove-address-btn removeItem removeRelatedEntities text-danger"><i class="fa fa-times-circle" aria-hidden="true"></i></a>';
        var tr = '<tr><td class="Sno" style="display: none;">' + (index + 1) + '</td><td>' + addressType + '</td><td>' + country + '</td><td style="width:145px;">' + address + '</td><td>' + city + '</td><td style="width:139px;">' + state  + '</td><td>' + zipcode + '</td><td class="text-center">' + action + '</td></tr>';
        $("#itemTableRelatedEntities tbody").append(tr);
        GetAddressType('companyAddressDTOList_' + index + '__AddressTypeId');
        GetCountry('companyAddressDTOList_' + index + '__CountryId');
        GetState('companyAddressDTOList_' + index + '__StateId');
        $('#errormetrc').html("");
    }

    loadScript();
}


function validateRelatedEntities(index) {
    var valid = true;
    for (var i = 0; i < index; i++) {
        valid = valid && validateRowRelatedEntities(i)
    }
    return valid;
}

function validateRowRelatedEntities(index) {

    var valid = true;
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__AddressType"));
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__Country"));
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__Address"));
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__City"));
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__State"));
    valid = valid && checkRelatedEntities($("#companyAddressDTOList_" + index + "__ZipCode"));
    return valid;
}

function checkRelatedEntities(o) {
    $("#error-form").html("");
    if (o.val() === "" || o.val() === null) {
        o.css('border', '1px solid #d9534f');
        o.closest('table').find('.error').remove();
        o.after('<span class="error" style="color:#d9534f;">' + o.attr('placeholder') + ' field is required</span>');
        //alert(o.attr('placeholder'));
        //$("#error-form").append('<tr><td colspan="5" style="color:#d9534f;"><span class="' + o.attr('placeholder') + '">' + o.attr('placeholder') + ' field is required</span></td></tr>');
        return false;
    } else {
        o.css('border', '1px solid #6d6d6d');
        return true;
    }
}
//End "RELATED ENTITIES" details and removed
function GetAddressType(ddlCustomers12) {
    var ddlCustomers1 = $("#" + ddlCustomers12);
    ddlCustomers1.empty().append('<option selected="selected" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: '../Employer/AddressType',
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          
            ddlCustomers1.empty().append();
            $.each(response, function () {
                ddlCustomers1.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }

    });

}
function GetCountry(ddlCountries) {
    debugger;
    var ddlcountry = $("#" + ddlCountries);
    ddlcountry.empty().append('<option selected="selected" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "POST",
        url: '../Employer/Country',
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            ddlcountry.empty().append();
            $.each(response, function () {
                ddlcountry.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function GetState(ddlStates) {
    debugger;
    var ddlstate = $("#" + ddlStates);
    ddlstate.empty().append('<option selected="selected" value="0" disabled = "disabled">Select State</option>');
    $.ajax({
        type: "POST",
        url: '../Employer/State',
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            ddlstate.empty().append();
            ddlstate.empty().append('<option selected="selected" value="0" disabled = "disabled">Select State</option>');
            $.each(response, function () {
               
                ddlstate.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}



function DeleteAddress(Id) {
    debugger
    DeleteConfirmation('btnaddressDelete');
    $('#btnaddressDelete').click(function () {
        $.ajax({
            url: "../Employer/DeleteAddressItem/" + Id,
            type: "GET",
            contentType: "application/json",
            async: true,
            success: function (data) {
                debugger;
                if (data == 'DeletedWork') {
                    $('#CommonDeleteModel').modal('hide');
                    window.location.reload();

                }
                else if (data == 'DeleteWorkFailure') {
                    $('#CommonDeleteModel').modal('hide');
                    LoadWorkData('Failure');
                }
            },
            error: function (xHr, status, res) {
                $('#pWarningMessage').empty().html(res);
                $('#CommonWarningModel').modal('show');
            }
        });
    });
}
$("#submit_form").click(function () {
    if (!validateRelatedEntities($('#itemTableRelatedEntities tbody tr').length)) {
        return false;
    }
});




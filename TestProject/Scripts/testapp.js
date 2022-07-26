// A $( document ).ready() block.
var apiBaseUrl = 'http://localhost:55749/api/directory/';
var getCurrentDirectory = apiBaseUrl + '' + 'GetCurrentDirectory';
var postFile = apiBaseUrl + '' + 'PostFile';
var htmlCurrentView = '';

$(document).ready(function () {
    LoadCurrentDirectory();
});

function LoadCurrentDirectory() {
    debugger;
    var qstring = GetParameterValues('directoryPath');    
    var url = getCurrentDirectory;
    if (qstring) {
        url = getCurrentDirectory + '?directoryPath=' + qstring;
    }
    var sText = $.trim($('#sText').val());
    if (sText && sText.length > 0) {
    }

    var data = { directoryPath: qstring, sText: sText };
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        data:data,
        success: function (data) {
            $('#container_fileList').empty();
            $('#container_currentDirectory').empty();
            $('#container_footer').empty();

            if (data && data.CurrentDirectory) {
                $('#container_currentDirectory').append('<h4>Path: ' + data.CurrentDirectory.Path + '</h4>');
                if (data.CurrentDirectory.Parent) {
                    $('#container_currentDirectory').append('<button type="button" id="backButton" onclick=updateQueryString("backButton") folderPath="' + data.CurrentDirectory.Parent + '">Back</button>');
                }

                $('#container_footer').append(' <label class="label">Subfolders</label><span class="label-val">' + data.CurrentDirectory.FolderCount + '</span ><label class="label">Files</label><span class="label-val">' + data.CurrentDirectory.FileCount + '</span>' + '<label class="label">Size</label><span class="label-val">' + data.CurrentDirectory.DirectorySize + '</span>');
            }

            /*$('#container_fileList').append('<h1>Folders....</h1>');*/
            if (data && data.Folders) {
                var list = '';
                $.each(data.Folders, function (index, value) {
                    console.log(value.FullPath);
                    list = list + '<li id="li_' + index + '" onclick=updateQueryString("li_' + index + '") folderPath="' + value.FullPath + '">' + value.Name + '</li>';
                });
                $('#container_fileList').append('<ul class="folder">' + list + '</ul>')
            }
            /*$('#container_fileList').append('<h1>Files....</h1>');*/
            if (data && data.Files) {
                var list = '';
                $.each(data.Files, function (index, value) {
                    console.log(value.FullPath);
                    list = list + '<li><a id="a_' + index + '" href="' + value.DownloadPath + '" target="_blank">' + value.FileName + '</a></li>';
                });
                $('#container_fileList').append('<ul class="files">' + list + '</ul>');
            }

        },
        error: function (request, message, error) {
            handleException(request, message, error);
            console.log(error);
        }
    });
}


//Common error handler
function handleException(request, message, error) {
    var msg = "";
    msg += "Code: " + request.status + "\n";
    msg += "Text: " + request.statusText + "\n";
    if (request.responseJSON != null) {
        msg += "Message: " +
            request.responseJSON.Message + "\n";
    }
    alert(msg);
    console.log(msg);
}


function updateQueryString(liId) {
    var fpath = $('#' + liId).attr('folderPath');
    $('#sText').val('');
    refreshQueryString(fpath);
}

function refreshQueryString(fpath) {
    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?directoryPath=' + $.trim(fpath);
        window.history.pushState({ path: newurl }, '', newurl);
    }
    LoadCurrentDirectory();
}

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

function searchDirectory() {   
    var sText = $.trim($('#sText').val());

    if (sText && sText.length > 0) {
        LoadCurrentDirectory();
    }
}

function UploadFile() {  
    var fd = new FormData();
    var files = $('#file')[0].files[0];
    fd.append('files', files);

    var qstring = GetParameterValues('directoryPath');    
    if (qstring) {
        fd.append("DirectoryPath", qstring);
    }
   

    $.ajax({
        url: postFile,
        type: 'post',
        data: fd,
        contentType: false,
        processData: false,
        success: function (response) {
            $('#file').val("");
            LoadCurrentDirectory();
        },
        error: function (request, message, error) {
            handleException(request, message, error);
            console.log(error);
        }
    });
}
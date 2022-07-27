var apiBaseUrl = 'http://localhost:55749/api/directory/';
var getCurrentDirectory = apiBaseUrl + '' + 'GetCurrentDirectory';
var postFile = apiBaseUrl + '' + 'PostFile';
var downloadFile = apiBaseUrl + '' + 'DownloadFile';

$(document).ready(function () {
    LoadCurrentDirectory();
});

function LoadCurrentDirectory() {
    var qstring = GetParameterValues('directoryPath');
    var url = getCurrentDirectory;
    if (qstring) {
        url = getCurrentDirectory + '?directoryPath=' + qstring;
    }
    var sText = $.trim($('#sText').val());


    var data = { directoryPath: qstring, sText: sText };
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        data: data,
        success: function (data) {
            $('#container_fileList').empty();
            $('#container_currentDirectory').empty();
            $('#container_footer').empty();

            //Show current directory
            if (data && data.CurrentDirectory) {
                $('#container_currentDirectory').append('<h4>Path: ' + data.CurrentDirectory.Path + '</h4>');
                if (sText && sText.length > 0) {
                    $('#container_currentDirectory').append('<p>Searched keyword: ' + sText + '<button class="searchButton" title="Clear search" onclick="clearSearch()">❌</button></p> ');
                }
                if (data.CurrentDirectory.Parent) {
                    $('#container_currentDirectory').append('<button type="button" id="backButton" onclick=LoadFolder("backButton") folderPath="' + data.CurrentDirectory.Parent + '"> ← Back</button>');
                }

                $('#container_footer').append(' <label class="label">Subfolders</label><span class="label-val">' + data.CurrentDirectory.FolderCount + '</span ><label class="label">Files</label><span class="label-val">' + data.CurrentDirectory.FileCount + '</span>' + '<label class="label">Size</label><span class="label-val">' + data.CurrentDirectory.DirectorySize + '</span>');
            }
            //Show folders
            if (data && data.Folders) {
                var list = '';
                $.each(data.Folders, function (index, value) {
                    console.log(value.FullPath);
                    list = list + '<li id="li_' + index + '" onclick=LoadFolder("li_' + index + '") folderPath="' + value.FullPath + '">' + value.Name + '</li>';
                });
                $('#container_fileList').append('<ul class="folder">' + list + '</ul>')
            }
            //Show files
            if (data && data.Files && data.Files.length > 0) {
                var list = '';
                $.each(data.Files, function (index, value) {
                    console.log(value.FullPath);
                    list = list + '<li><a href="javascript:void(0)" id="a_' + index + '" filePath="' + value.DownloadPath + '" onclick=DownloadFile("a_' + index + '")>' + value.FileName + '</a></li>';
                });
                $('#container_fileList').append('<ul class="files">' + list + '</ul>');
            } else {
                if (sText && sText.length > 0) {
                    $('#container_fileList').append('<p>No files found with searched keyword!</p>');
                }
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


function LoadFolder(liId) {
    var fpath = $('#' + liId).attr('folderPath');
    $('#sText').val('');
    refreshQueryString(fpath);
}

//update the query string for deeplinking
function refreshQueryString(fpath) {
    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?directoryPath=' + $.trim(fpath);
        window.history.pushState({ path: newurl }, '', newurl);
    }
    LoadCurrentDirectory();
}

//get Query string params
function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

//Button handler to search directory
function searchDirectory() {
    var sText = $.trim($('#sText').val());

    if (sText && sText.length > 0) {
        LoadCurrentDirectory();
    }
}

//Button handler to upload file
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

//Clear button handler to clear the search
function clearSearch() {
    $('#sText').val('');
    LoadCurrentDirectory();
}

//Link handler to download file
function DownloadFile(fileToDownload) {
    var fpath = $('#' + fileToDownload).attr('filePath');
    var req = new XMLHttpRequest();
    req.open("GET", downloadFile + '?filePath=' + fpath, true);
    req.responseType = "blob";

    req.onload = function (event) {
        var blob = req.response;
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = $('#' + fileToDownload).text();
        link.click();
    };

    req.send();
}
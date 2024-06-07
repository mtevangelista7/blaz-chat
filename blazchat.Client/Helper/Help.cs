using System.Globalization;
using blazchat.Client.Components.Dialogs.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace blazchat.Client.Helper;

public static class Help
{
    public static string? PathFile { get; set; }

    /// <summary>
    /// show a dialog with a message
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="message"></param>
    public static async Task ShowAlertDialog(IDialogService dialogService, string message)
    {
        var dialog =
            await dialogService.ShowAsync<AlertDialog>("Alert", new DialogParameters { { "Message", message } });
        var result = await dialog.Result;
    }

    /// <summary>
    /// show a dialog with a message and return a boolean
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static async Task<bool> ShowConfirmDialog(IDialogService dialogService, string message)
    {
        var dialog =
            await dialogService.ShowAsync<ConfirmDialog>("Confirm", new DialogParameters { { "Message", message } });
        var result = await dialog.Result;

        return result.Canceled;
    }

    /// <summary>
    /// save the error message in a file and show a dialog with the error message
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="exception"></param>
    /// <param name="component"></param>
    public static async Task HandleError(IDialogService dialogService, Exception exception, object component)
    {
        var dialog =
            await dialogService.ShowAsync<AlertDialog>("Error",
                new DialogParameters { { "Message", exception.Message } });
        var result = await dialog.Result;

        if (component is ComponentBase componentBase)
        {
        }

        await WriteLog(exception.Message);
    }

    /// <summary>
    /// save the error message in a file
    /// </summary>
    /// <param name="exception"></param>
    public static async Task HandleError(Exception exception)
    {
        await WriteLog(exception.Message);
    }

    /// <summary>
    /// write the error message in a file
    /// </summary>
    /// <param name="message"></param>
    private static async Task WriteLog(string message)
    {
        if (!Directory.Exists(PathFile))
        {
            Directory.CreateDirectory(PathFile);
        }

        var newPath = Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now.Date}-ErrorLog.txt");
        var file = File.OpenWrite(newPath);
        await using var sw = new StreamWriter(file);
        await sw.WriteLineAsync(DateTime.Now.ToString("HH:mm:ss"));
        await sw.WriteAsync(message);
        await sw.WriteAsync("--------------------------------------------------------------------");
    }
}
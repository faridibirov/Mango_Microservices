using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging;

public class AzureServiceBusConsumer:IAzureServiceBusConsumer
{
    private readonly string serviceBusConnectionString;
    private readonly string emailCartQueue;
    private readonly string registerUserQueue;
    private readonly IConfiguration _configuration;
    private EmailService _emailService;
    private ServiceBusProcessor _emailCartProcessor;
    private ServiceBusProcessor _registerUserProcessor;

    public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailSender)
    {
        _emailService = emailSender;
        _configuration = configuration;
        serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
        emailCartQueue = _configuration.GetValue<string>("TopicAndQueuNames:EmailShoppingCartQueue");
        registerUserQueue = _configuration.GetValue<string>("TopicAndQueuNames:RegisterUserQueue");
        var client = new ServiceBusClient(serviceBusConnectionString);
        _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        _registerUserProcessor = client.CreateProcessor(emailCartQueue);
    }

    public async Task Start()
    {
        _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
        await _emailCartProcessor.StartProcessingAsync();

        _registerUserProcessor.ProcessMessageAsync += OnRegisterNewUserReceived;
        _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
        await _registerUserProcessor.StartProcessingAsync();
    }

    public async Task Stop()
    {
        await _emailCartProcessor.StopProcessingAsync();
        await _emailCartProcessor.DisposeAsync();

        await _registerUserProcessor.StopProcessingAsync();
        await _registerUserProcessor.DisposeAsync();
    }

    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
    {
        // this is where you will receive message
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
        try
        {
            //TODO - try to log email
            await _emailService.EmailCartAndLog(objMessage);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task OnRegisterNewUserReceived(ProcessMessageEventArgs args)
    {
        // this is where you will receive message
        var message = args.Message;
        var body = Encoding.UTF8.GetString(message.Body);

        string email = JsonConvert.DeserializeObject<string>(body);
        try
        {
            //TODO - try to log email
            await _emailService.RegisterUserEmailAndLog(email);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private  Task ErrorHandler(ProcessErrorEventArgs args)
    {
       Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

   
}

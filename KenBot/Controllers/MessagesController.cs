using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Ken
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        //public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        //{
        //    if (activity.Type == ActivityTypes.Message)
        //    {
        //        var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

        //        var replies = CreateReply(activity);
        //        foreach (var reply in replies)
        //        {
        //            await connector.Conversations.ReplyToActivityAsync(reply);
        //        }
        //    }
        //    else
        //    {
        //        HandleSystemMessage(activity);
        //    }
        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    return response;
        //}

        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new KenAi());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private static IEnumerable<Activity> CreateReply(Activity activity)
        {
            var replies = new List<Activity>();
            var txt = activity.Text;

            if (string.IsNullOrEmpty(txt))
            {
                replies.Add(activity.CreateReply("You sent empty text"));
                return replies;
            }

            if (txt.StartsWith("SEND:"))
            {
                EikonCi.Instance.Value.SendMessage("Hi", txt);
                replies.Add(activity.CreateReply($"Sent \"{txt}\" to EikonCI"));
                return replies;
            }

            if (txt.StartsWith("DEBUG:"))
            {
                var reply = activity.CreateReply();
                reply.TextFormat = "markdown";

                var text = new StringBuilder();
                text.AppendLine("# DEBUG");
                text.AppendLine($"**FromId**: {activity.From.Id}");
                text.AppendLine();
                text.AppendLine($"**FromName**: {activity.From.Name}");
                text.AppendLine();
                text.AppendLine($"**RecipientId**: {activity.Recipient.Id}");
                text.AppendLine();
                text.AppendLine($"**RecipientName**: {activity.Recipient.Name}");
                text.AppendLine();
                text.AppendLine($"**ConversationId**: {activity.Conversation.Id}");
                text.AppendLine();
                text.AppendLine($"**ConversationName**: {activity.Conversation.Name}");
                text.AppendLine();
                text.AppendLine($"**ConversationIsGroup**: {activity.Conversation.IsGroup}");
                text.AppendLine();
                text.AppendLine($"**Text**: {activity.Text}");
                text.AppendLine();
                text.AppendLine($"**ChannelId**: {activity.ChannelId}");
                text.AppendLine();
                text.AppendLine($"**ServiceUrl**: {activity.ServiceUrl}");
                text.AppendLine();
                text.AppendLine($"**Id**: {activity.Id}");
                text.AppendLine();
                text.AppendLine($"**Summary**: {activity.Summary}");
                text.AppendLine();
                text.AppendLine($"**Locale**: {activity.Locale}");
                text.AppendLine();
                text.AppendLine($"**Timestamp**: {activity.Timestamp}");
                text.AppendLine();
                text.AppendLine($"**TopicName**: {activity.TopicName}");
                reply.Text = text.ToString();

                replies.Add(reply);
                return replies;
            }

            // calculate something for us to return
            var length = txt.Length;
            replies.Add(activity.CreateReply($"You sent {txt} which was {length} characters"));
            return replies;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Discord365.UI.Extensions;

namespace Discord365.UI.User.MessagesPage.Message
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : UserControl
    {
        private Discord.IUser relatedUser = null;
        public DateTime TimeStamp
        {
            get => MessageHeader.TimeStamp;
            set => MessageHeader.TimeStamp = value;
        }

        public Message()
        {
            InitializeComponent();
        }

        public Discord.IUser RelatedUser
        {
            get => relatedUser;
            set
            {
                relatedUser = value;

                MessageHeader.RelatedUser = value;
            }
        }

        public void AppendMessage(Discord.IMessage msg)
        {
            SingleMessage m = new SingleMessage { Message = msg };
            MessagesPanel.Children.Add(m);

            if (msg.Content.Length >= 1)
            {
                var c = new TextMessageContent
                {
                    Margin = DefaultPadding,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    MessageText = msg.Content
                };
                m.Panel.Children.Add(c);
            }

            if(msg.Attachments.Count >= 1)
            {
                foreach(var attachment in msg.Attachments)
                {
                    if (attachment.Filename.CheckForEnding(ImageExtensions))
                    {
                        var img = new MessageImage(attachment.Filename)
                        {
                            Margin = DefaultPadding,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        img.SetImage(attachment.Url);
                        m.Panel.Children.Add(img);
                    }
                    else
                    {
                        var aobj = new AttachmentContent
                        {
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Attach = attachment,
                            Margin = DefaultPadding
                        };
                        m.Panel.Children.Add(aobj);
                    }
                }
            }

            if(msg.Embeds.Count >= 1)
            {
                foreach (var embed in msg.Embeds)
                {
                    var aobj = new MessageEmbed
                    {
                        Related = embed,
                        Margin = DefaultPadding
                    };
                    m.Panel.Children.Add(aobj);
                }
            }
        }

        public Thickness DefaultPadding => new Thickness(0, 2, 0, 2);

        public void AddImageMessage(Discord.IMessage msg)
        {

        }

        public void RemoveMessage(SocketMessage msg)
        {
            foreach (var m in MessagesPanel.Children)
            {
                SingleMessage s = m as SingleMessage;

                if (s.Message == msg)
                    MessagesPanel.Children.Remove(s);
            }
        }
    }
}

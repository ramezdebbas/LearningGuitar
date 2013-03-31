using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "The guitar is a string instrument of the chordophone family constructed from wood and strung with either nylon or steel strings. The modern guitar was preceded by the lute, vihuela, four-course renaissance guitar and five-course baroque guitar, all of which contributed to the development of the modern six-string instrument.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Getting Started",
                    "Getting Started Playing Guitar So, you've decided that you want to learn how to play the guitar! Unfortunately, it can seem very daunting to start. You might stare at the guitar and not have any idea what you're supposed to do with it! It's time to lose the fear and fulfill your dream of becoming a guitarist today.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nGetting Started Playing Guitar So, you've decided that you want to learn how to play the guitar! Unfortunately, it can seem very daunting to start. You might stare at the guitar and not have any idea what you're supposed to do with it! It's time to lose the fear and fulfill your dream of becoming a guitarist today.\n\nThe first thing you need to do is think about why you want to learn to play the guitar. Defining your goals will help you follow through with them. There are countless numbers of people out there who decide that they want to play the guitar, but who never actually follow through with it. Or, if they do, they quickly give up and move onto something else. Understanding the reasons why you want to play the guitar will help you stick with it and become a better player!\n\nAfter you've done that, you need to look into getting some equipment. You'll need some direction for this, as there are many different types of guitars out there. Still, don't let the options overwhelm you. A simple acoustic guitar is all anyone really needs to get started. Other simple tools like an electric tuner will help immensely. Of course, after you've gotten your equipment you'll need to take a course to learn how to play. Many people balk at the thought of that since hiring a private teacher can be quite expensive. There may be classes in your area, but they can also be expensive and you may not be able to find the time to drive there and back consistently. For many people, a better idea is to teach themselves. This is easier said than done though -- unless you have the right course.Many people make the mistake of picking up a simple book on playing guitar, or a singular instructional video, and expect to become masterful right away. It takes more than that! Thankfully, there are some great online guitar tutoring options that can really work for you.\n\nThe reason that learning to play the guitar online works so well is because it covers some new learning modalities. You can read information, watch videos, and listen to audios. You can also work with interactive software to be sure all of the information is sinking in. It's also good to know that the best online courses won't cost you a fortune. These are quite comprehensive and can quickly and easily take you into the realm of becoming an experienced, and good, guitar player. There is no reason to delay! Start learning how to play the guitar today, and finally fulfill your dream.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Getting Started", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Learning Guitar" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Things to know",
                     "Things You Must Know Before You Start Playing Since you've decided that you are serious about learning how to play the guitar, it's important to understand some of the most crucial aspects of this. First of all, know that practicing is one of the most important things when it comes to your guitar playing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThings You Must Know Before You Start Playing Since you've decided that you are serious about learning how to play the guitar, it's important to understand some of the most crucial aspects of this. First of all, know that practicing is one of the most important things when it comes to your guitar playing. It's not enough to just play all the time, you need to be consistently and actively improving your skills.\n\nYou also need to make sure that you do not get frustrated with yourself. When you see more advanced players, you might feel like you should be able to do the same things. Realize that everyone has to start somewhere, and there is no shame in being a beginner right now. If you have the right instruction, you will quickly move up in the ranks of guitar players. For now, pay close attention to your technique and do everything you can do right way so you can become an expert player in no time. \n\nPlease remember that becoming a good player takes commitment. If you have motivating instruction, this will likely not be an issue. The problems start when you're trying to attend a course somewhere, and you simply can't make it, so you get frustrated because you fall behind in your lessons. Or,you just do not understand what is being taught. That's why online courses can be so effective -- they hit on all of the learning modalities, and you're a lot less likely to become frustrated.\n\nSince you have made this commitment, you are probably very interested in all the different guitar types out there. There are some really fabulous guitars! Many of these are quite expensive, so consider what kind of budget you have right now. You may want to start out with something simple and work your way up later as you become a better player. It can be quite fun to grow your guitar collection, but that is also something that can happen over time rather than right away. This takes some dedication and commitment. Motivate yourself to learn how to play the guitar, and the chances are good that you will pick up the lessons quickly and easily. Your friends and family members won't believe how quickly you are able to learn to play the guitar! Imagine being able to impress everyone at parties, and being able to enjoy yourself by picking up the guitar and playing whenever you want to. Make it happen!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Things to know", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Choose Guitar",
                     "One of the most important decisions you'll make as a beginner is which guitar you should go with. There are an incredible number of options out there, in a variety of different price ranges. No matter how much you are willing to spend right now, there are some key things you will need to keep in mind.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nOne of the most important decisions you'll make as a beginner is which guitar you should go with. There are an incredible number of options out there, in a variety of different price ranges. No matter how much you are willing to spend right now, there are some key things you will need to keep in mind.\n\nOne is that you can find some really great deals on guitars. Just know that you need something high quality. It will be overly frustrating for you to try to play well on a poor instrument!\n\nWhile you might be motivated to save money, know that if you have a guitar you really like you will likely be more motivated to continue playing. You will look forward to holding the guitar and practicing and playing. Do yourself a favor and get a catalog full of different guitars to drool over -- and possibly even go in to the shop to try different ones. You just might find yourself falling in love with a certain guitar. There are some truly beautiful ones out there for great prices, and you can find them if you do your homework.\n\nAnother consideration is getting the proper size. Smaller adults and children will desperately need to take this into consideration. There are smaller and full-size guitars available, depending on your size and your needs. Getting something that fits just right will make a big difference.\n\nWhile you want to make sure you get a guitar you will really like, beware of spending your life savings on a guitar before you really get good at playing. While some people know they will be dedicated to the guitar for a lifetime, other people will end up choosing a different hobby. You'll be less likely to frustrate yourself if you settle with a guitar you can afford for now. \n\nGetting a guitar has a lot to do with feeling. You'll probably know what guitar is right for you as soon as you see it. Keep in mind that you'll also need some accessories to go with your new guitar. Picks and strings are essential (strings can break, so get extras!). You'll also need an electric tuner. This will ensure you have a high quality sound when you start strumming your guitar. Choosing your guitar is a very exciting event. Now you are ready to really begin learning how to play the guitar. From this point on, you can consider yourself a true guitarist!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Choose Guitar", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Learn to play",
                     "How Will You Learn to Play? Now that you have your guitar, and you've decided that you want to become a guitarist, it's time to choose how you will learn to play the guitar. Previously in this series, we touched on some of the available options.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow Will You Learn to Play? Now that you have your guitar, and you've decided that you want to become a guitarist, it's time to choose how you will learn to play the guitar. Previously in this series, we touched on some of the available options. These are learning in person with a professional teacher, learning on your own with a book or video, or learning with an online interactive course. \n\nTraditionally, many people hire a personal guitar teacher in order to learn. This is still a very viable option, and there are many people offering their services as guitar teachers. This is a great way to learn since you will receive personalized attention. Unfortunately, it can be very expensive. It can also be very difficult to find time to fit in these private lessons if you have a hectic and busy schedule.\nFor that reason, many people decide to teach themselves the guitar -- at least at first. Many people's first thought is to run out and get a book on learning the guitar. While this can help to a certain extent, they are definitely not comprehensive enough to turn you into a true guitar player. At best, you'll likely easily become frustrated learning from a book.\n\nThere are also videos you can learn from. This is good because you can visually see what an instructor is doing, and you can also watch the video on your own time. However, they also leave a lot to be desired because you cannot go in depth, ask questions, or read written material. There are different learning modalities, and using one method simply doesn't hit all of them. Thankfully, the Internet makes it easier than ever to learn to play the guitar. There are some great online guitar teaching courses for you to choose from. These courses work because they have a handle on all of the different the learning modalities. You'll be able to read information, watch videos to see instructors playing, listen to audio, play along with audio, and even use interactive games and software.\n\nThis works so well because you will be motivated to practice. It's not just one method of learning, so you will never get bored since there are so many different opportunities to learn. The information will also stand out more in your mind since it has come from a variety of different sources.\n\nThink about your current lifestyle and what will fit in best with your needs. Also, examine what you're able to afford financially, as well as what kind of time commitment you are able to make. Many people will decide that an online interactive course is simply the best method for them!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Learn to play", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Practice",
                     "Guitar Practice Really Does Make Perfect When many people start out learning how to play the guitar they are very involved and excited about it. Unfortunately, that interest often starts to wane and the practice sessions dwindle.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nGuitar Practice Really Does Make Perfect When many people start out learning how to play the guitar they are very involved and excited about it. Unfortunately, that interest often starts to wane and the practice sessions dwindle. That's when many guitars end up sitting in a dusty corner! Don't let that happen to you, and make sure that you are always motivated to practice. The first thing you should realize is that practice really does make perfect. The more you practice, and really apply what you are learning, the better you will get. The better you get, the more likely you will be motivated to continue playing. It's rough at first because you may not see yourself making progress. However, if you keep on going you will make more progress than you ever thought possible and you will only continue to get better. It might seem daunting at first, but it really is worth it in the end.\n\nYou should really devote yourself to playing for at least 10 minutes per day -- more if possible. This is a short enough amount of time that you won't feel intimidated by putting it into your schedule. Everyone can afford to spend 10 minutes doing something they love and want to do! It's also enough time to really get in some great practice. Of course, if you are able to practice for longer amounts of time, you should. Doing this consistently and making it an important part of your life will make you more motivated to continue doing it.\n\nOne of the best ways to motivate yourself to continue practicing is to take the right guitar learning course. You should stick with something that is very interactive and comes at you from a variety of different angles. That way you'll never get bored and you'll be constantly learning how to play better. For instance, you can watch videos to follow along with experts, play along with an audio, and use interactive software to improve your skills. Doing these things will embed the skills in your mind and you will learn a lot more quickly.\n\nAs you're learning and practicing, always make sure that you are doing things right. Perfecting your technique and skill is very important -- and that means you don't want to rush things. That's part of what makes practicing so important. When you practice, you're really focusing on the technical aspects. When you're simply playing, you probably aren't focused on those things as much. That's why you should get as much practice and play time in as you can.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Practice", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Next Level",
                     "Take Your Guitar Playing to the Next Level Hopefully, you've gotten started playing guitar, and are very motivated to practice. As you get better, there are some things you'll need to keep in mind as you try to get to that next level.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTake Your Guitar Playing to the Next Level Hopefully, you've gotten started playing guitar, and are very motivated to practice. As you get better, there are some things you'll need to keep in mind as you try to get to that next level. These tips will help you become a great and dedicated guitar player. The first step is a little boring, but is also very necessary. It's that you should never try and rush things. You may hear some awesome players, but if you aren't at their skill level yet you should stick with where you are. You need to learn a new technique and practice it before you move on. You should master the skills as you go. This will set the foundation for you as a great guitar player in the future.\n\nAnother thing you should do is make sure you do not develop bad habits along the way. Examine your posture and how you hold the guitar. A good course will teach you how to do these things when you get started so those bad habits will never have a chance to develop. In addition to all of that, you should pay close attention to your guitar and keep it in tip top shape.\n\nAbove all, keep yourself motivated even when you feel your interests start to wane. There are those difficult patches where you don't feel like you're getting any better and you may even become frustrated. A great way to avoid this is to start talking to other guitar players. It's good to talk to those who are even more advanced than you are, so they may be able to help you through the rough patches. Of course, if you have the right course, this is not something you'll often have to deal with! There are some great online guitar playing communities that can really help you. As you observe and watch others, you will get better yourself. Standing on the shoulders of guitar playing giants will help take you to the next level as well. It's all about immersing yourself in the world of guitar and becoming truly great.\n\nHopefully you're becoming very excited about playing the guitar. You have your equipment picked out, you have chosen how you are going to learn, and you have motivated yourself to keep on going. When you do these things, you'll be able to learn faster than you ever thought possible. Getting to the next level is simply a matter of always being able to learn and grow.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Next Level", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Read Music",
                     "Reading Music -- Eliminate the Intimidation One thing that holds many guitar players back from really getting started is the fear of reading music. Sure, you may have taken some music classes in elementary school, but if that's as far as it went, you may not remember anything you learned!",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nReading Music -- Eliminate the Intimidation One thing that holds many guitar players back from really getting started is the fear of reading music. Sure, you may have taken some music classes in elementary school, but if that's as far as it went, you may not remember anything you learned! The good news is that with the right course you will learn exactly what you need to know about reading music. You will also come to understand the difference between reading notes and guitar tabs. This will not be comprehensive course on learning to read music in and of itself, but it will hopefully show you that there is nothing to be afraid of. First, you'll notice that there are five horizontal lines. This is just there to show you how high or low a note is. Pay close attention to where the dots fall on the line.\n\nBasically, all you need to know is where the notes are on the guitar, and where they are on these horizontal lines. Once you know that, it will be smooth sailing. Yes, it can seem a little bit like learning a new language right now, but it actually will not take you long. A lot of guitar music is written in tablature instead. This is often easier to recognize and use for guitar players (especially in the beginning!). Still, always keep in the back of your mind that it is actually not difficult to read music notes. Don't let this scare you off, and it can actually be quite fun to learn. You do not need to jump into learning how to read music right away. However, if you do remember a little bit from your music classes, or perhaps you would know how to read music for other reasons, then you can certainly incorporated into your guitar playing.\n\nThis is just one of the many skills you will pick up as you become a guitar player. Hopefully you do not feel intimidated -- when all the different skills you have to learn are presented in front of you it can certainly seem intimidating! Education is key, and that is why it is so important to have a comprehensive course to guide you through. That way you never have to worry or wonder about any aspect of learning how to play the guitar.\n\nLearning how to read music is one of the many components you'll encounter. Simply understand what those lines and dots mean, and you are well under way to becoming an expert guitar player. For now, don't let them scare you off!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Read Music", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "CutOut to play",
                     "Are You Really Cut Out to Play the Guitar? Unfortunately, many people start to doubt themselves as they begin to play the guitar. It's not due to a lack of motivation for many, it's due to a lack of confidence.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAre You Really Cut Out to Play the Guitar? Unfortunately, many people start to doubt themselves as they begin to play the guitar. It's not due to a lack of motivation for many, it's due to a lack of confidence. Keep in mind that if you put your mind to it you can certainly learn to play guitar quickly and easily!It's time to banish the fear out of your mind. As you start to go through the lessons, it will all become clear to you. You will understand how to play the guitar if that's what you want to do. Music is not some mystical thing! It's always difficult to learn something new at first, but then everything will start to click. It's really sad that so many people give up become because of this fear, but by educating yourself you will realize that you can avoid it.\n\nIt's also never too late to learn to play the guitar. Perhaps you've started and stopped this process many times before. Perhaps it was a long-term goal for you to learn to play, but you were always afraid or weren't sure that you are cut out for it. Once again, it's time to stop the fear, and to take action with playing the guitar today!No matter how many times you've given up on this or something else in the past, you can change what happens in the future. You can become a guitar player!\n\nTo make things easier for you, there are a huge variety of learning choices out there. You can go to a private class, learn from a book or video, or even take an online interactive course. Many people are opting for the online interactive courses these days. That's because there are video, written, audio, and software components. The best ones even include personal one-on-one instruction as part of the cost of the product. It's a wonderful, economic option that will help to motivate you even further.\n\nWhat's great about a comprehensive course is that you can learn at your own pace. There's no need to feel like you have to drop your entire life schedule, or feel a sense of being rushed. Also, if you consider yourself a fast learner, you can go through the lessons as quickly as you would like. This works out well for many people.\n\nYou are definitely cut out to play the guitar! There is no reason to delay since there are some truly great options for helping you learn how to play.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "CotOut to play", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Learning Hard",
                     "Learning to Play the Guitar is Not as Hard as You Think! When you're learning how to play the guitar it can seem impossibly hard. Just keep in mind that everyone has to start somewhere -- everyone was a beginner at some point in time!",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nLearning to Play the Guitar is Not as Hard as You Think! When you're learning how to play the guitar it can seem impossibly hard. Just keep in mind that everyone has to start somewhere -- everyone was a beginner at some point in time! It's time for you to move from those fearful beginning stages and into experienced guitar playing now. The great thing is that you can learn to play guitar even if you've never considered yourself to be musically inclined before. All it takes is the right instruction and the right motivation. You also need to dedicate yourself to practicing. The chances are good that even if you don't think you have a musical bone in your body, you can quickly and easily learn to play the guitar. All you need is a love of music and a desire to play.\n\nWhatever method you decide to learn from, you need to ensure it hits all of the learning modalities. It's not enough to just go to the library and check out a book on how to play the guitar. This will likely bore you, and you won't get as much out of it. People learn in different ways. Some people are visual learners, some people are auditory learners, and some people are kinesthetic learners. Most people are a combination.\n\nOnline guitar courses are great these days because they are geared toward all of the learning modalities. You'll get to hear real guitar players, and play along with them, watch them on video, and interact with software and games. Doing all of these different things will help to cement the information in your mind. You will also be super motivated to follow through with the information. This is a very important thing when you're learning how to play the guitar. In addition to hitting all those learning modalities, you'll always be able to get some one-on-one instruction. Not everyone can afford a private tutor, so you'll be glad to know that the best online courses also include a coaching component. If you've ever have a question, or feel like you're getting something wrong, there's someone you can turn to.\n\nYou should also know that there are people out there just like you. There are beginners who are intimidated right now and need a helping hand. You can find these people in your local community, and even online via forums. Online forums are great because you can talk with other guitar enthusiasts from all over the world. All of these things help to ensure that learning to play the guitar is not as hard as you think. It's time to take action today and to banish that fear!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Learning Hard", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Learning Guitar" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Take Actions",
                     "Take Action With Playing the Guitar Today It's time to move from the dream of playing the guitar, into the reality of actually doing so! Way too many people think that some day it will be the right time for them to play.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTake Action With Playing the Guitar Today It's time to move from the dream of playing the guitar, into the reality of actually doing so! Way too many people think that some day it will be the right time for them to play. That right time, is right now.The first thing you need to do is decide how you're going to learn. There are a few different options, with the best being online interactive courses, for a variety of reasons. From that point, you'll be able to get down the basics.\n\nThen, you can choose your guitar -- this will become your babyand a truly prized possession. Be sure to take the time to choose a guitar that will really be right for you.After you've gotten the guitar, and the other odds and ends you'll need (such as an electric tuner, picks, and strings), it's time to take action, and start learning today. If you've chosen the right course, it will take you all the way from these beginning stages, straight through to more advanced material. This is a great thing, that will make learning easier than ever.\n\nYou will probably find that you start to gravitate toward a particular way of learning. You might like reading text better -- or perhaps you'll like watching videos better. Just be sure that you follow through with all the different methods because they will help you learn. The more different angles you learn from, the more likely you are to remember and get a grasp on the material.\n\nAlways remember that in addition to learning the material, you need to put it into practice. That means more than just playing -- you need to actually apply what you've learned. Practice really does make perfect! You can take as much, or as little time as you need, but just be sure you always set aside some time every day. That is the only way you'll learn to become an awesome guitar player. \n\nAs you progress through your lessons, you'll probably become more and more motivated to succeed. It works like a snowball effect! Your success will breed more success, and it will be an amazing feeling. There is nothing quite like succeeding with your mission to become a guitar player! If you take action today, that dream can truly become a reality. All you need is the right instrument, and the right course to guide you through. There is no time like the present to succeed with your dreams!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Take Actions", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Learning Guitar" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}

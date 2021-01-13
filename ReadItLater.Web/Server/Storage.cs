using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReadItLater.Data;

namespace ReadItLater.Web.Server
{
    public static class Storage
    {
        public static ICollection<Tag> Tags { get; } = new List<Tag>();
        public static ICollection<Folder> Folders { get; } = new List<Folder>();
        public static Folder DefaultFolder => Folders.Single(x => x.Name.Equals("coding"));
        public static ICollection<Ref> Refs { get; } = new List<Ref>();
    }

    public static class Seed
    {
        public static void Tags()
        {
            var tags = new Tag[]
            {
                new Tag { Id = Guid.Parse("ab773010-d644-42f7-990e-9b1919e09a66"), Name = "proglib" },
                new Tag { Id = Guid.Parse("0027add5-4278-49ed-93fe-a85323425453"), Name = "tproger" },
                new Tag { Id = Guid.Parse("4ad68193-cf3e-4da5-9db2-080250966103"), Name = "DotNext" },
                new Tag { Id = Guid.Parse("aa6f9657-6e36-4013-b5ce-3856354367bf"), Name = "youtube" },
                new Tag { Id = Guid.Parse("1a148330-e0c4-4c81-b50e-208004540545"), Name = "delegate"},
                new Tag { Id = Guid.Parse("2c2c646a-1df3-4370-a639-69d2f9fafd92"), Name = "c# 8" },
                new Tag { Id = Guid.Parse("f65d32f5-9a67-4873-9b4c-576dbda6bd0d"), Name = "c#" }
            };

            tags.ToList().ForEach(x => Storage.Tags.Add(x));
        }

        public static void Folders()
        {
            var coding = new Folder
            {
                Id = Guid.Parse("6a26aa74-5fa5-4047-a0ca-096bf8128e62"),
                Name = "coding",
                Folders = new Folder[]
                {
                    new Folder { Name = ".net" },
                    new Folder { Name = "ai" },
                    new Folder { Name = "other" }
                }
            };
            var hardware = new Folder { Id = Guid.Parse("41504905-a14d-407d-adc0-cc296252ff03"), Name = "hardware" };
            var business = new Folder { Id = Guid.Parse("4383bad3-bc25-421f-aea8-7f891c3a48a0"), Name = "business" };
            var travel = new Folder { Id = Guid.Parse("08ff4b2f-c682-46f3-8362-c756abf423a5"), Name = "travel" };
            var entertainment = new Folder { Id = Guid.Parse("b2394c22-fce1-4106-b7f6-d13af4d56f5d"), Name = "entertainment" };

            var folders = new Folder[]
            {
                coding, hardware, business, travel, entertainment
            };

            folders.ToList().ForEach(x => Storage.Folders.Add(x));
        }

        public static void Refs()
        {
            var Coding = Storage.DefaultFolder;
            var refs = new Ref[]
            {
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "How Delegates Work in C# - Master C# with Productive C#",
                    Url = "https://proglib.io/w/7347c55d",
                    Image = "https://i2.wp.com/www.productivecsharp.com/wp-content/uploads/2019/11/How-Delegates-Work-in-C-Thumbnail.jpg?fit=1280%2C720&#038;ssl=1",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("proglib")),
                        Storage.Tags.Single(x => x.Name.Equals("delegate"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "Interfaces in C# 8.0 gets a makeover",
                    Url = "https://proglib.io/w/6a3e5c41",
                    Image = "https://www.talkingdotnet.com/wp-content/uploads/2019/09/Interfaces-in-C-8.png",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("proglib")),
                        Storage.Tags.Single(x => x.Name.Equals("c# 8"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "C# to C# Communication: REST, gRPC and everything in between",
                    Url = "https://proglib.io/w/a5ce5546",
                    Image = "https://i2.wp.com/michaelscodingspot.com/wp-content/uploads/2020/02/c-to-c-rest-vs-grpc.jpg?fit=1000%2C640&#038;ssl=1",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("proglib"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "Конечный автомат: теория и реализаци",
                    Url = "https://tproger.ru/translations/finite-state-machines-theory-and-implementation/",
                    Image = "https://s3.tproger.ru/uploads/2015/09/finite-automata.jpg",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("proglib"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "ASP.NET Core приложения под Linux в продакшене",
                    Url = "https://youtu.be/ngcigr_8oxw",
                    Image = "https://i.ytimg.com/vi/ngcigr_8oxw/maxresdefault.jpg",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("youtube")),
                        Storage.Tags.Single(x => x.Name.Equals("DotNext"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "Building Full-stack C# Web Apps with Blazor in .NET Core 3.0",
                    Url = "https://youtu.be/zYeVH_g7ZHo",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Image = "https://i.ytimg.com/vi/zYeVH_g7ZHo/maxresdefault.jpg",
                    Tags = new Tag[]
                    {
                        Storage.Tags.Single(x => x.Name.Equals("youtube"))
                    },
                    Priority = Priority.Middle
                },
                new Ref
                {
                    FolderId = Coding.Id,
                    Title = "Logging in C# .NET Modern-day Practices: The Complete Guide",
                    Url = "https://michaelscodingspot.com/logging-in-dotnet/",
                    Date = DateTime.Now.AddDays(new Random().Next(100) * -1),
                    Image = "https://michaelscodingspot.com/wp-content/uploads/2019/08/logging-frameworks-complete-guide.jpg",
                    Tags = new Tag[] { },
                    Priority = Priority.Middle
                }
            };

            refs.ToList().ForEach(x => Storage.Refs.Add(x));
        }
    }
}
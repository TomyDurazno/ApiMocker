using GraphQL.Net;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SQLite.CodeFirst;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GraphQLMocker.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public async Task<object> Get()
        {
            var input = await Request.Content.ReadAsStringAsync();

            string query = Regex.Unescape(input).Replace("\n",string.Empty);

            query = query.Replace(@"\", string.Empty);

            var j = JObject.Parse(query);

            var qs = j["query"].ToString();
            
            //var q2 = qs.Substring(1, qs.Length - 2).Trim();

            var regex = new Regex("{query:(.*)}");

            var q = regex.Match(query).Groups[1].ToString();

            EntityFrameworkExecutionTests.Init();

            /*
            var ef = new EntityFrameworkExecutionTests();

            var methods = ef.GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(TestAttribute), true).Any())
                .Select(m => new { m.Name, Exec = (bool)m.Invoke(ef, null) })
                .ToList();

            var tests = methods.GroupBy(n => n.Exec);
            */

            var context = EntityFrameworkExecutionTests.CreateDefaultContext();

            var result = context.ExecuteQuery(qs);

            return result;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    public static class Test
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer
        {
            Converters = { new StringEnumConverter() },
        };
        public static bool DeepEquals(IDictionary<string, object> results, string json)
        {
            var expected = JObject.Parse(json);
            var actual = JObject.FromObject(results, Serializer);
            if (expected.ToString() == actual.ToString())
                return true;
            else
                return false;
            //throw new Exception($"Results do not match expectation:\n\nExpected:\n{expected}\n\nActual:\n{actual}");
        }
    }

    public static class GenericTests
    {
        public static bool LookupSingleEntity<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, name } }");
            return Test.DeepEquals(results, "{ user: { id: 1, name: 'Joe User' } }");
        }

        public static bool AliasOneField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { idAlias : id, name } }");
            return Test.DeepEquals(results, "{ user: { idAlias: 1, name: 'Joe User' } }");
        }

        public static bool NestedEntity<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, account { id, name } } }");
            return Test.DeepEquals(results, "{ user: { id: 1, account: { id: 1, name: 'My Test Account' } } }");
        }

        public static bool NoUserQueryReturnsNull<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:0) { id, account { id, name } } }");
            return Test.DeepEquals(results, "{ user: null }");
        }

        public static bool CustomFieldSubQuery<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, accountPaid } }");
            return Test.DeepEquals(results, "{ user: { id: 1, accountPaid: true } }");
        }

        public static bool CustomFieldSubQueryUsingContext<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, total } }");
            return Test.DeepEquals(results, "{ user: { id: 1, total: 2 } }");
        }

        public static bool List<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ users { id, name } }");
            return Test.DeepEquals(results, "{ users: [{ id: 1, name: 'Joe User'}, { id: 2, name: 'Late Paying User' }] }");
        }

        //public static bool ListTypeIsList<TContext>(GraphQL<TContext> gql)
        //{
        //    var users = gql.ExecuteQuery("{ users { id, name } }")["users"];
        //    Assert.AreEqual(users.GetType(), typeof(List<IDictionary<string, object>>));
        //}

        public static bool NestedEntityList<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { id, users { id, name } } }");
            return Test.DeepEquals(results, "{ account: { id: 1, users: [{ id: 1, name: 'Joe User' }] } }");
        }

        public static bool PostField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, abc } }");
            return Test.DeepEquals(results, "{ user: { id: 1, abc: 'easy as 123' } }");
        }

        public static bool PostFieldSubQuery<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { sub { id } } }");
            return Test.DeepEquals(results, "{ user: { sub: { id: 1 } } }");
        }

        public static bool TypeName<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, __typename } }");
            return Test.DeepEquals(results, "{ user: { id: 1, __typename: 'User' } }");
        }

        public static bool DateTimeFilter<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ accountPaidBy(paid: { year: 2016 month: 1 day: 1 }) { id } }");
            return Test.DeepEquals(results, "{ accountPaidBy: { id: 1 } }");
        }

        public static bool EnumerableSubField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { activeUsers { id, name } } }");
            return Test.DeepEquals(results, "{ account: { activeUsers: [{ id: 1, name: 'Joe User' }] } }");

            var results2 = gql.ExecuteQuery("{ account(id:2) { activeUsers { id, name } } }");
            return Test.DeepEquals(results2, "{ account: { activeUsers: [] } }");
        }

        public static bool SimpleMutation<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("mutation { mutate(id:1,newVal:5) { id, value } }");
            return Test.DeepEquals(results, "{ mutate: { id: 1, value: 5 } }");

            var results2 = gql.ExecuteQuery("mutation { mutate(id:1,newVal:123) { id, value } }");
            return Test.DeepEquals(results2, "{ mutate: { id: 1, value: 123 } }");
        }

        public static bool MutationWithReturn<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("mutation { addMutate(newVal: 7) { value } }");
            return Test.DeepEquals(results, "{ addMutate: { value: 7 } }");
        }

        public static bool NullPropagation<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ user(id:1) { id, nullRef { id } } }");
            return Test.DeepEquals(results, "{ user: { id: 1, nullRef: null } }");
        }

        public static bool GuidField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { id, someGuid } }");
            return Test.DeepEquals(results, "{ account: { id: 1, someGuid: '00000000-0000-0000-0000-000000000000' } }");
        }

        public static bool GuidParameter<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ accountsByGuid(guid:\"00000000-0000-0000-0000-000000000000\") { id, someGuid } }");
            return Test.DeepEquals(results, @"{
                                           accountsByGuid: [
                                               { id: 1, someGuid: '00000000-0000-0000-0000-000000000000' },
                                               { id: 2, someGuid: '00000000-0000-0000-0000-000000000000' },
                                           ]
                                       }");
        }

        public static bool EnumFieldQuery<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ accountsByType(accountType:accountType_Gold) { id, accountType } }");
            return Test.DeepEquals(results, @"{ 
                                            accountsByType: [
                                                    { id: 1, accountType: 'Gold' }
                                            ]
                                        }");
        }

        public static bool ByteArrayParameter<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { id, byteArray } }");
            return Test.DeepEquals(results, "{ account: { id: 1, byteArray: 'AQIDBA==' } }"); // [1, 2, 3, 4] serialized to base64 by Json.NET
        }

        public static bool ChildListFieldWithParameters<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { id, name, usersWithActive(active:true) { id, name } } }");
            return Test.DeepEquals(results, "{ account: { id: 1, name: 'My Test Account', usersWithActive: [{ id: 1, name: 'Joe User' }] } }");

            results = gql.ExecuteQuery("{ account(id:1) { id, name, usersWithActive(active:false) { id, name } } }");
            return Test.DeepEquals(results, "{ account: { id: 1, name: 'My Test Account', usersWithActive: [] } }");
        }

        public static bool ChildFieldWithParameters<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ account(id:1) { id, name, firstUserWithActive(active:true) { id, name } } }");
            return Test.DeepEquals(results, "{ account: { id: 1, name: 'My Test Account', firstUserWithActive: { id: 1, name: 'Joe User' } } }");

            results = gql.ExecuteQuery("{ account(id:1) { id, name, firstUserWithActive(active:false) { id, name } } }");
            return Test.DeepEquals(results, "{ account: { id: 1, name: 'My Test Account', firstUserWithActive: null } }");
        }

        public static bool Fragements<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { name, __typename, ...human, ...stormtrooper, ...droid } }, " +
                "fragment human on Human { height }, " +
                "fragment stormtrooper on Stormtrooper { specialization }, " +
                "fragment droid on Droid { primaryFunction }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo', __typename: 'Human',  height: 5.6430448}, " +
                "{ name: 'FN-2187', __typename: 'Stormtrooper',  height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2', __typename: 'Droid', primaryFunction: 'Astromech' } ] }"
                );
        }

        public static bool InlineFragements<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { name, __typename, ... on Human { height }, ... on Stormtrooper { specialization }, " +
                "... on Droid { primaryFunction } } }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo', __typename: 'Human',  height: 5.6430448}, " +
                "{ name: 'FN-2187', __typename: 'Stormtrooper',  height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2', __typename: 'Droid', primaryFunction: 'Astromech' } ] }"
                );
        }

        public static bool InlineFragementWithListField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { name, __typename, ... on Human { height, vehicles { name } }, ... on Stormtrooper { specialization }, " +
                "... on Droid { primaryFunction } } }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo', __typename: 'Human',  height: 5.6430448, vehicles: [ {name: 'Millennium falcon'}] }, " +
                "{ name: 'FN-2187', __typename: 'Stormtrooper',  height: 4.9, vehicles: [ {name: 'Speeder bike'}], specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2', __typename: 'Droid', primaryFunction: 'Astromech' } ] }"
                );
        }

        public static bool FragementWithMultiLevelInheritance<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ heros { name, __typename, ... on Stormtrooper { height, specialization } } }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo', __typename: 'Human'}, " +
                "{ name: 'FN-2187', __typename: 'Stormtrooper',  height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2', __typename: 'Droid' } ] }"
                );
        }

        public static bool InlineFragementWithoutTypenameField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ heros { name, ... on Stormtrooper { height, specialization } } }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo'}, " +
                "{ name: 'FN-2187', height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2' } ] }"
                );
        }

        public static bool InlineFragementWithoutTypenameFieldWithoutOtherFields<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery("{ heros { ... on Stormtrooper { height, specialization } } }");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ }, " +
                "{ height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ } ] }"
                );
        }

        public static bool FragementWithoutTypenameField<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { name, ...stormtrooper } }, fragment stormtrooper on Stormtrooper { height, specialization } ");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo',}, " +
                "{ name: 'FN-2187', height: 4.9, specialization: 'Imperial Snowtrooper'}, " +
                "{ name: 'R2-D2', } ] }"
                );
        }

        public static bool FragementWithMultipleTypenameFields<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { name, ...stormtrooper, __typename } }, fragment stormtrooper on Stormtrooper { height, specialization, __typename } ");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ name: 'Han Solo', __typename: 'Human'}, " +
                "{ name: 'FN-2187', height: 4.9, specialization: 'Imperial Snowtrooper', __typename: 'Stormtrooper'}, " +
                "{ name: 'R2-D2', __typename: 'Droid'} ] }"
                );
        }

        public static bool FragementWithMultipleTypenameFieldsMixedWithInlineFragment<TContext>(GraphQL<TContext> gql)
        {
            var results = gql.ExecuteQuery(
                "{ heros { ...stormtrooper, __typename, ... on Human {name}, ... on Droid {name}}}, fragment stormtrooper on Stormtrooper { name, height, specialization, __typename } ");
            return Test.DeepEquals(
                results,
                "{ heros: [ " +
                "{ __typename: 'Human',  name: 'Han Solo'}, " +
                "{ name: 'FN-2187', height: 4.9, specialization: 'Imperial Snowtrooper', __typename: 'Stormtrooper'}, " +
                "{ __typename: 'Droid', name: 'R2-D2'} ] }"
                );
        }

        //public static void LookupSingleEntityError<TContext>(GraphQL<TContext> gql)
        //{
        //    Assert.Throws<ValidationException>(() => gql.ExecuteQuery("{ userByName(name:JoeUser) { id, name } }"));
        //}
    }

    public class EntityFrameworkExecutionTests
    {
        public static void Init()
        {
            using (var db = new EfContext())
            {
                if (db.Accounts.Any())
                    return;

                var account = new Account
                {
                    Name = "My Test Account",
                    Paid = true,
                    PaidUtc = new DateTime(2016, 1, 1),
                    AccountType = AccountType.Gold
                };
                db.Accounts.Add(account);
                var user = new User
                {
                    Name = "Joe User",
                    Account = account,
                    Active = true,
                };
                db.Users.Add(user);
                var account2 = new Account
                {
                    Name = "Another Test Account",
                    Paid = false,
                    AccountType = AccountType.Silver
                };
                db.Accounts.Add(account2);
                var user2 = new User
                {
                    Name = "Late Paying User",
                    Account = account2
                };
                db.Users.Add(user2);
                db.MutateMes.Add(new MutateMe());

                var human = new Human
                {
                    Id = 1,
                    Name = "Han Solo",
                    Height = 5.6430448
                };
                db.Heros.Add(human);
                var stormtrooper = new Stormtrooper
                {
                    Id = 2,
                    Name = "FN-2187",
                    Height = 4.9,
                    Specialization = "Imperial Snowtrooper"
                };
                db.Heros.Add(stormtrooper);
                var droid = new Droid
                {
                    Id = 3,
                    Name = "R2-D2",
                    PrimaryFunction = "Astromech"
                };
                db.Heros.Add(droid);
                var vehicle = new Vehicle
                {
                    Id = 1,
                    Name = "Millennium falcon",
                    Human = human
                };
                db.Vehicles.Add(vehicle);

                var vehicle2 = new Vehicle
                {
                    Id = 2,
                    Name = "Speeder bike",
                    Human = stormtrooper
                };
                db.Vehicles.Add(vehicle2);

                db.SaveChanges();
            }
        }

        public static GraphQL<EfContext> CreateDefaultContext()
        {
            var schema = GraphQL<EfContext>.CreateDefaultSchema(() => new EfContext());
            schema.AddScalar(new { year = 0, month = 0, day = 0 }, ymd => new DateTime(ymd.year, ymd.month, ymd.day));
            InitializeUserSchema(schema);
            InitializeAccountSchema(schema);
            InitializeMutationSchema(schema);
            InitializeNullRefSchema(schema);
            InitializeCharacterSchema(schema);
            schema.Complete();
            return new GraphQL<EfContext>(schema);
        }

        private static void InitializeUserSchema(GraphQLSchema<EfContext> schema)
        {
            var user = schema.AddType<User>();
            user.AddField(u => u.Id);
            user.AddField(u => u.Name);
            user.AddField(u => u.Account);
            user.AddField(u => u.NullRef);
            user.AddField("total", (db, u) => db.Users.Count());
            user.AddField("accountPaid", (db, u) => u.Account.Paid);
            user.AddPostField("abc", () => GetAbcPostField());
            user.AddPostField("sub", () => new Sub { Id = 1 });

            schema.AddType<Sub>().AddField(s => s.Id);
            schema.AddListField("users", db => db.Users);
            schema.AddField("user", new { id = 0 }, (db, args) => db.Users.FirstOrDefault(u => u.Id == args.id));
            schema.AddField("userByName", new { name = "" }, (db, args) => db.Users.FirstOrDefault(u => u.Name == args.name));
        }

        private static string GetAbcPostField() => "easy as 123"; // mimic an in-memory function

        private static void InitializeAccountSchema(GraphQLSchema<EfContext> schema)
        {
            var account = schema.AddType<Account>();
            account.AddField(a => a.Id);
            account.AddField(a => a.Name);
            account.AddField(a => a.Paid);
            account.AddField(a => a.SomeGuid);
            account.AddField(a => a.ByteArray);
            account.AddField(a => a.AccountType);
            account.AddListField(a => a.Users);
            account.AddListField("activeUsers", (db, a) => a.Users.Where(u => u.Active));
            account.AddListField("usersWithActive", new { active = false }, (db, args, a) => a.Users.Where(u => u.Active == args.active));
            account.AddField("firstUserWithActive", new { active = false }, (db, args, a) => a.Users.FirstOrDefault(u => u.Active == args.active));

            schema.AddField("account", new { id = 0 }, (db, args) => db.Accounts.FirstOrDefault(a => a.Id == args.id));
            schema.AddField
                ("accountPaidBy", new { paid = default(DateTime) },
                    (db, args) => db.Accounts.AsQueryable().FirstOrDefault(a => a.PaidUtc <= args.paid));
            schema.AddListField("accountsByGuid", new { guid = Guid.Empty },
                    (db, args) => db.Accounts.AsQueryable().Where(a => a.SomeGuid == args.guid));
            schema.AddListField("accountsByType", new { accountType = AccountType.None },
                    (db, args) => db.Accounts.AsQueryable().Where(a => a.AccountType == args.accountType));
            schema.AddEnum<AccountType>(prefix: "accountType_");
            //add this enum just so it is part of the schema
            schema.AddEnum<MaterialType>(prefix: "materialType_");
        }

        private static void InitializeMutationSchema(GraphQLSchema<EfContext> schema)
        {
            var mutate = schema.AddType<MutateMe>();
            mutate.AddAllFields();

            schema.AddField("mutateMes", new { id = 0 }, (db, args) => db.MutateMes.AsQueryable().FirstOrDefault(a => a.Id == args.id));
            schema.AddMutation("mutate",
                new { id = 0, newVal = 0 },
                (db, args) =>
                {
                    var mutateMe = db.MutateMes.First(m => m.Id == args.id);
                    mutateMe.Value = args.newVal;
                    db.SaveChanges();
                },
                (db, args) => db.MutateMes.AsQueryable().FirstOrDefault(a => a.Id == args.id));
            schema.AddMutation("addMutate",
                new { newVal = 0 },
                (db, args) =>
                {
                    var newMutate = new MutateMe { Value = args.newVal };
                    db.MutateMes.Add(newMutate);
                    db.SaveChanges();
                    return newMutate.Id;
                },
                (db, args, id) => db.MutateMes.AsQueryable().FirstOrDefault(a => a.Id == id));
        }

        private static void InitializeNullRefSchema(GraphQLSchema<EfContext> schema)
        {
            var nullRef = schema.AddType<NullRef>();
            nullRef.AddField(n => n.Id);
        }

        private static void InitializeCharacterSchema(GraphQLSchema<EfContext> schema)
        {
            schema.AddType<Character>().AddAllFields();
            schema.AddType<Human>().AddAllFields();
            schema.AddType<Stormtrooper>().AddAllFields();
            schema.AddType<Droid>().AddAllFields();
            schema.AddType<Vehicle>().AddAllFields();

            schema.AddField("hero", new { id = 0 }, (db, args) => db.Heros.SingleOrDefault(h => h.Id == args.id));
            schema.AddListField("heros", db => db.Heros.AsQueryable());
        }

        [Test]
        public bool LookupSingleEntity() => GenericTests.LookupSingleEntity(CreateDefaultContext());
        [Test]
        public bool AliasOneField() => GenericTests.AliasOneField(CreateDefaultContext());
        [Test]
        public bool NestedEntity() => GenericTests.NestedEntity(CreateDefaultContext());
        [Test]
        public bool NoUserQueryReturnsNull() => GenericTests.NoUserQueryReturnsNull(CreateDefaultContext());
        [Test]
        public bool CustomFieldSubQuery() => GenericTests.CustomFieldSubQuery(CreateDefaultContext());
        [Test]
        public bool CustomFieldSubQueryUsingContext() => GenericTests.CustomFieldSubQueryUsingContext(CreateDefaultContext());
        [Test]
        public bool List() => GenericTests.List(CreateDefaultContext());
        [Test]
        public bool NestedEntityList() => GenericTests.NestedEntityList(CreateDefaultContext());
        [Test]
        public bool PostField() => GenericTests.PostField(CreateDefaultContext());
        [Test]
        public bool PostFieldSubQuery() => GenericTests.PostFieldSubQuery(CreateDefaultContext());
        [Test]
        public bool TypeName() => GenericTests.TypeName(CreateDefaultContext());
        [Test]
        public bool DateTimeFilter() => GenericTests.DateTimeFilter(CreateDefaultContext());
        [Test]
        public bool EnumerableSubField() => GenericTests.EnumerableSubField(CreateDefaultContext());
        [Test]
        public bool SimpleMutation() => GenericTests.SimpleMutation(CreateDefaultContext());
        [Test]
        public bool MutationWithReturn() => GenericTests.MutationWithReturn(CreateDefaultContext());
        [Test]
        public bool NullPropagation() => GenericTests.NullPropagation(CreateDefaultContext());
        [Test]
        public bool GuidField() => GenericTests.GuidField(CreateDefaultContext());
        [Test]
        public bool GuidParameter() => GenericTests.GuidParameter(CreateDefaultContext());
        [Test]
        public bool EnumFieldQuery() => GenericTests.EnumFieldQuery(CreateDefaultContext());
        [Test]
        public bool ByteArrayParameter() => GenericTests.ByteArrayParameter(CreateDefaultContext());
        //[Test]
        //public bool ChildListFieldWithParameters() => GenericTests.ChildListFieldWithParameters(MemContext.CreateDefaultContext());
        //[Test]
        //public bool ChildFieldWithParameters() => GenericTests.ChildFieldWithParameters(MemContext.CreateDefaultContext());
        [Test]
        public static bool Fragements() => GenericTests.Fragements(CreateDefaultContext());
        [Test]
        public static bool InlineFragements() => GenericTests.InlineFragements(CreateDefaultContext());
        [Test]
        public static bool InlineFragementWithListField() => GenericTests.InlineFragementWithListField(CreateDefaultContext());
        [Test]
        public static bool FragementWithMultiLevelInheritance() => GenericTests.FragementWithMultiLevelInheritance(CreateDefaultContext());
        [Test]
        public static bool InlineFragementWithoutTypenameField() => GenericTests.InlineFragementWithoutTypenameField(CreateDefaultContext());
        [Test]
        public static bool FragementWithoutTypenameField() => GenericTests.FragementWithoutTypenameField(CreateDefaultContext());
        [Test]
        public static bool InlineFragementWithoutTypenameFieldWithoutOtherFields() => GenericTests.InlineFragementWithoutTypenameFieldWithoutOtherFields(CreateDefaultContext());
        [Test]
        public static bool FragementWithMultipleTypenameFields() => GenericTests.FragementWithMultipleTypenameFields(CreateDefaultContext());
        [Test]
        public static bool FragementWithMultipleTypenameFieldsMixedWithInlineFragment() => GenericTests.FragementWithMultipleTypenameFieldsMixedWithInlineFragment(CreateDefaultContext());

        //[Test]
        //public void AddAllFields()
        //{
        //    var schema = GraphQL<EfContext>.CreateDefaultSchema(() => new EfContext());
        //    schema.AddType<User>().AddAllFields();
        //    schema.AddType<Account>().AddAllFields();
        //    schema.AddField("user", new { id = 0 }, (db, args) => db.Users.FirstOrDefault(u => u.Id == args.id));
        //    schema.Complete();

        //    var gql = new GraphQL<EfContext>(schema);
        //    var results = gql.ExecuteQuery("{ user(id:1) { id, name } }");
        //    Test.DeepEquals(results, "{ user: { id: 1, name: 'Joe User' } }");
        //}

        //public void LookupSingleEntityError() => GenericTests.LookupSingleEntityError(CreateDefaultContext());

        public enum AccountType
        {
            None,
            Silver,
            Gold,
            Platinum
        }

        public enum MaterialType
        {
            None,
            Iron,
            Plastic,
            Wood,
            Silver,
            Gold,
            Platinum
        }


        class Sub
        {
            public int Id { get; set; }
        }

        public class EfContext : DbContext
        {
            static EfContext()
            {
                // This is necessary to make the SQLite provider work with Guids
                Environment.SetEnvironmentVariable("AppendManifestToken_SQLiteProviderManifest", ";BinaryGUID=True;");
            }

            public EfContext() : base("DefaultConnection") { }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                Database.SetInitializer(new SqliteDropCreateDatabaseWhenModelChanges<EfContext>(modelBuilder));
                base.OnModelCreating(modelBuilder);
            }

            public IDbSet<User> Users { get; set; }
            public IDbSet<Account> Accounts { get; set; }
            public IDbSet<MutateMe> MutateMes { get; set; }
            public IDbSet<NullRef> NullRefs { get; set; }
            public IDbSet<Character> Heros { get; set; }
            public IDbSet<Vehicle> Vehicles { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Active { get; set; }

            public int AccountId { get; set; }
            public Account Account { get; set; }

            public int? NullRefId { get; set; }
            public NullRef NullRef { get; set; }
        }

        public class Account
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Paid { get; set; }
            public DateTime? PaidUtc { get; set; }
            public Guid SomeGuid { get; set; }
            public byte[] ByteArray { get; set; } = { 1, 2, 3, 4 };

            public AccountType AccountType { get; set; }

            public List<User> Users { get; set; }
        }

        public class MutateMe
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }

        public class NullRef
        {
            public int Id { get; set; }
        }

        public class Character
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public  class Human : Character
        {
            public double Height { get; set; }
            public ICollection<Vehicle> Vehicles { get; set; }
        }

        public class Stormtrooper : Human
        {
            public string Specialization { get; set; }
        }

        public class Droid : Character
        {
            public string PrimaryFunction { get; set; }
        }

        public class Vehicle
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int HumanId { get; set; }
            public virtual Human Human { get; set; }
        }
    }
}

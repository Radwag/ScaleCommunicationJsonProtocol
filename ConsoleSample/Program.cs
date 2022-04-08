using System;
using ConsoleSample.JsonRdgProtocol;

//Initialize JsonRdgProtocol
JsonRdgProtocol jsonRdgProtocol = new JsonRdgProtocol();

// Display title as the C# console app.
Console.WriteLine("\nSample Scale Communication Json Protocol in C# core\r");
Console.WriteLine("------------------------\n");

// Connect to device.
jsonRdgProtocol.Connect();

//initialize data model
jsonRdgProtocol.GetTables(false);
jsonRdgProtocol.GetDbInfos(false);

loop :
// Ask the user to choose an option.
Console.WriteLine("Choose an option from the following list:");
Console.WriteLine("\t1 - List tables from database");
Console.WriteLine("\t2 - Get Table Info by table name");
Console.WriteLine("\t3 - List records from table");
Console.WriteLine("\t4 - Get record by Id from table");
Console.WriteLine("\t5 - Add record to table");
Console.WriteLine("\t6 - Update record from table use defined key");
Console.WriteLine("\t7 - Delete record from database");
Console.WriteLine("\t8 - Types and Alterability");
Console.WriteLine("\t0 - Exit");

Console.Write("Your option? ");
Console.WriteLine("------------------------\n");

// Use a switch statement to do the math.
switch (Console.ReadLine())
{
    case "1": //List tables from database
        jsonRdgProtocol.GetTables(true);
        Console.Write("Press key to continue...");
        Console.ReadKey();
        goto loop;

    case "2": //Get Table Info by table name
        case2 :
        Console.Write("Select table:\n");
        //List all tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.GetDbInfo(jsonRdgProtocol.Tables[int.Parse(i)].Name, true);
                }
                catch
                {
                    goto case2;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "3": //List record from table
        case3 :
        Console.Write("Select table:\n");
        //List all tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.GetDbInfo(jsonRdgProtocol.Tables[int.Parse(i)].Name, true);
                    jsonRdgProtocol.ListRecordsFromTable(jsonRdgProtocol.Tables[int.Parse(i)].Name);
                }
                catch
                {
                    goto case3;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "4": //Get record by Id from table
        case4 :
        Console.Write("Select table:\n");
        //List all tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.GetDbInfo(jsonRdgProtocol.Tables[int.Parse(i)].Name, true);
                    Console.Write("Enter record Id:");
                    var id = Int64.Parse(Console.ReadLine()!);
                    jsonRdgProtocol.GetRecordById(jsonRdgProtocol.Tables[int.Parse(i)].Name, id);
                }
                catch
                {
                    Console.Write("The entered data is incorrect.\n");
                    goto case4;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "5": //Add record to table
        case5 :
        Console.Write("Select table:\n");

        //List editable tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            if (jsonRdgProtocol.Tables[i].Alterability == 0)
            {
                Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
            }
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.AddRecordToTable(jsonRdgProtocol.Tables[int.Parse(i)].Name);
                }
                catch
                {
                    Console.Write("The entered data is incorrect.\n");
                    goto case5;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "6": //Update record by Id from database ,use Name key
        //List editable tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            if (jsonRdgProtocol.Tables[i].Alterability == 0)
            {
                Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
            }
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.UpdateRecordFromTable(jsonRdgProtocol.Tables[int.Parse(i)].Name);
                }
                catch
                {
                    Console.Write("The entered data is incorrect.\n");
                    goto case5;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "7": //Delete record from database
        //List editable tables
        for (var i = 0; i < jsonRdgProtocol.Tables.Count; i++)
        {
            if (jsonRdgProtocol.Tables[i].Alterability == 0)
            {
                Console.WriteLine("\t" + i + " - " + jsonRdgProtocol.Tables[i].Name);
            }
        }

        switch (Console.ReadLine())
        {
            case { } i:
                try
                {
                    jsonRdgProtocol.DeleteRecordFromTable(jsonRdgProtocol.Tables[int.Parse(i)].Name);
                }
                catch
                {
                    Console.Write("The entered data is incorrect.\n");
                    goto case5;
                }

                Console.Write("Press key to continue...");
                Console.ReadKey();
                goto loop;
        }

        break;

    case "8": //Types and visibility
        Console.Write(
            "Data Types:\n" +
            "Index	Name\n" +
            "0	UNSET\n" +
            "1	Catalog\n" +
            "2	Tex\n" +
            "3	Floating\n" +
            "4	Enum\n" +
            "5	TimeSpan\n" +
            "6	DateTime\n" +
            "7	Int\n" +
            "8	OneToOneRel\n" +
            "9	OneToManyRel\n" +
            "10	Bool\n" +
            "11	Password\n" +
            "12	Ean13\n" +
            "13	MultiLineText\n" +
            "14	PrimaryKey\n" +
            "15	Structure\n" +
            "16	Mask\n");
        Console.WriteLine("------------------------\n");

        Console.WriteLine(
            "Alterability:\n" +
            "Index	Name\n" +
            "0	Visible, ReadOnly=false\n" +
            "1	Visible, ReadOnly=true\n" +
            "2	Hide, ReadOnly=false\n");

        Console.Write("Press key to continue...");
        Console.ReadKey();
        goto loop;

    case "0":
        jsonRdgProtocol.PerformDisconnection();
        Environment.Exit(0);
        break;
    default: goto loop;
}
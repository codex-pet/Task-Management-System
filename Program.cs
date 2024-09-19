using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


class TaskManager
{
    private List<TaskItem> itemCol;
    private string filePath;

    public TaskManager(string fileName)
    {
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "TextFiles");
        Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist
        filePath = Path.Combine(directoryPath, fileName);
        this.itemCol = new List<TaskItem>();
    }

    public void LoadTasksFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                itemCol = JsonSerializer.Deserialize<List<TaskItem>>(jsonData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading tasks from file: " + ex.Message);
        }
    }

    public void SaveTasksToFile()
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(itemCol, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("Serialized JSON data:");
            Console.WriteLine(jsonData); // Debugging output
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving tasks to file: " + ex.Message);
        }
    }
}
class TaskItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("priority")]
    public int Priority { get; set; }

    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("estimatedTime")]
    public TimeSpan EstimatedTime { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; }

    [JsonPropertyName("isArchived")]
    public bool IsArchived { get; set; }

    public TaskItem(string name, int priority, bool isCompleted, DateTime dueDate, string category, TimeSpan estimatedTime, string notes, bool isArchived)
    {
        Name = name;
        Priority = priority;
        IsCompleted = false;
        DueDate = dueDate;
        Category = category;
        EstimatedTime = estimatedTime;
        Notes = notes;
        IsArchived = false;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public void ArchiveTask()
    {
        IsArchived = true;
    }
}

class Program
{
    static TaskManager taskManager;
    static string filePath = "TextFiles//tasks.txt"; 
    static void Main()
    {
        taskManager = new TaskManager("tasks.txt");

        Console.ForegroundColor = ConsoleColor.Green;
        int loadingLeft = (Console.WindowWidth - "Loading".Length) / 2;
        int loadingTop = Console.WindowHeight / 2;

        int dotsLeft = loadingLeft + "Loading".Length;
        int dotsTop = loadingTop;

        Console.SetCursorPosition(loadingLeft, loadingTop);
        Console.Write("Loading\t");

        Console.SetCursorPosition(dotsLeft, dotsTop);

        for (int i = 0; i < 10; i++)
        {
            Console.Write("▄\t");
            Thread.Sleep(300);
        }

        Console.Clear();

        Console.Title = "Task Management Program";
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.SetWindowSize(80, 30);


        Console.WriteLine("\t\t\t\t   TO-DO LIST");

        List<TaskItem> itemCol = new List<TaskItem>();
        List<TaskItem> tasks = new List<TaskItem>();
        List<TaskItem> archivedTasks = new List<TaskItem>();

        DisplayTaskCount(itemCol);

        string options = " ";

        while (options != "e")
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\nThese are your options: ");
            Console.WriteLine("\t[1] - View Current Task TODO");
            Console.WriteLine("\t[2] - Add New Task");
            Console.WriteLine("\t[3] - Remove Task");
            Console.WriteLine("\t[4] - Set Task as Completed");
            Console.WriteLine("\t[5] - View Sorted Task (Priority/Completion/DueDate)");
            Console.WriteLine("\t[6] - Set New Due Date");
            Console.WriteLine("\t[7] - Set New Category/Label");
            Console.WriteLine("\t[8] - Set New Priority");
            Console.WriteLine("\t[9] - Add/Edit Notes");
            Console.WriteLine("\t[10] - Search and Filtering Task");
            Console.WriteLine("\t[11] - Archive Task");
            Console.WriteLine("\t[e] - To Exit the Program");

            bool isValidInput = false;
            while (!isValidInput)
            {
                options = Console.ReadLine();

                if (options == "1" || options == "2" || options == "3" || options == "4" || options == "5"  || options == "6"  || options == "7"|| options == "8" || options == "9" || options == "10" || options == "11" || options == "e")
                {
                    isValidInput = true; 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid option (1-11) or 'e' to exit.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue; 
                }
            }

            if (options == "1")  //Viewing Task
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNo Task Available");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Current Task TODO:");

                    int count = 1;

                    foreach (var task in itemCol)
                    {
                        string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                        Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category}, Status: {task.EstimatedTime}) \nNote: {task.Notes}");
                        Console.ResetColor();
                        count++;
                    }
                }
            }

            else if (options == "2")  // Add New Task
            {
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("How many tasks would you like to input? ");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    int numberOfTasks;
                    while (!int.TryParse(Console.ReadLine(), out numberOfTasks) || numberOfTasks <= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Invalid input. Please enter a valid number of tasks: ");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }

                    for (int i = 0; i < numberOfTasks; i++)
                    {
                        AddNewTask(itemCol);
                    }
                }
                Console.WriteLine("Total tasks in itemCol: " + itemCol.Count);
                taskManager.SaveTasksToFile();
            }

            else if (options == "3")  // Remove Task
            {
                Console.Clear();
                DisplayTaskCount(itemCol);
                Console.WriteLine("Current Task TODO:");

                int count = 1;

                foreach (var task in itemCol)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus})");
                    Console.ResetColor();
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("\nRemove Task: \n");
                int removeItem;

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                while (!int.TryParse(Console.ReadLine(), out removeItem) || removeItem < 1 || removeItem > itemCol.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input. Please enter a valid task number.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }              

                TaskItem taskToRemove = itemCol[removeItem - 1];

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Are you sure you want to remove the task \"{taskToRemove.Name}\"? (Y/N)");
                Console.ForegroundColor = ConsoleColor.DarkBlue;

                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "y")
                {
                    itemCol.RemoveAt(removeItem - 1); 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task successfully removed from the list.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else if (confirmation == "n")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Task deletion canceled.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Task deletion canceled.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            }

            else if (options == "4") // Set Task as Completed
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Current Task TODO:");

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue;
                }            

                int count = 1;

                foreach (var task in itemCol)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus})");
                    Console.ResetColor();
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\nEnter the task number to mark as completed:");
                int taskNumber;

                if (int.TryParse(Console.ReadLine(), out taskNumber) && taskNumber >= 1 && taskNumber <= itemCol.Count)
                {
                    TaskItem taskToComplete = itemCol[taskNumber - 1];
                    
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Are you sure you want to mark the task \"{taskToComplete.Name}\" as completed? (Y/N)");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "y")
                    {
                        taskToComplete.MarkAsCompleted();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Task marked as completed.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (confirmation == "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Task completion canceled.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Task completion canceled.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid task number. Please try again.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            } 

            else if (options == "5") // View Sorted Task
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Input a task to be sorted by [1]Priority, [2]Completed, [3]DueDate: ");
                Console.ResetColor();
                string sortOption = Console.ReadLine();

                var sortedTasks = itemCol;

                switch (sortOption)
                {
                    case "1":
                        sortedTasks = itemCol.OrderBy(task => task.Priority).ToList();
                        break;
                    case "2":
                        sortedTasks = itemCol.OrderBy(task => task.IsCompleted).ToList();
                        break;
                    case "3":
                        sortedTasks = itemCol.OrderBy(task => task.DueDate).ToList();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid sorting option.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;
                }

                int count = 1;

                foreach (var task in sortedTasks)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category}) Notes: {task.Notes}");
                    Console.ResetColor();
                    count++;
                }
            }

            else if (options == "6") // Set Due Date
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Current Task TODO:");

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                int count = 1;

                foreach (var task in itemCol)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category})");
                    Console.ResetColor();
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Enter the task number to reset due date:");

                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= itemCol.Count)
                {
                    Console.WriteLine("Enter New Due date for the task (YYYY-MM-DD):");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
                    {
                        itemCol[taskNumber - 1].DueDate = dueDate;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The Due date set successfully!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid date format. Please use YYYY-MM-DD format.");
                    }
                } 
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid task number. Please try again.");
                }
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
            }

            else if (options == "7")  // Set Category
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Current Task TODO:");

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                    int count = 1;

                    foreach (var task in itemCol)
                    {
                        string completionStatus = task.IsCompleted ? "Completed" : "Not Completed";
                        Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category})");
                        Console.ResetColor();
                        count++;
                    }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Enter the task number to set the new category:");

                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= itemCol.Count)
                {
                    Console.WriteLine("Enter new category for the task:");
                    string category = Console.ReadLine();
                    itemCol[taskNumber - 1].Category = category;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Category set successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid task number. Please try again.");
                }
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }

            else if (options == "8") // Set Priority
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Current Task TODO:");

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                int count = 1;

                foreach (var task in itemCol)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed";
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category})");
                    Console.ResetColor();
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Enter the task number to set the new priority:");
                

                if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= itemCol.Count)
                {
                    Console.WriteLine("Enter new priority for the task:");
                    int priority = Convert.ToInt32(Console.ReadLine());
                    itemCol[taskNumber - 1].Priority = priority;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("The Priority set successfully!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid task number. Please try again.");
                }
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }

            else if (options == "9")  // Add/Edit Notes
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Current Task TODO:");

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                int count = 1;

                foreach (var task in itemCol)
                {
                    string completionStatus = task.IsCompleted ? "Completed" : "Not Completed";
                    Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Status: {task.DueDate}, Status: {task.Category}) Note: {task.Notes}");
                    Console.ResetColor();
                    count++;
                }

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Enter the task number to edit the notes:");
                int taskNumber;
                
                while (!int.TryParse(Console.ReadLine(), out taskNumber) || taskNumber < 1 || taskNumber > itemCol.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid task number:");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }

                TaskItem selectedTask = itemCol[taskNumber - 1];

                Console.WriteLine($"Current notes for {selectedTask.Name}: {selectedTask.Notes}");
                Console.WriteLine("Enter new notes:");
                string newNotes = Console.ReadLine();

                selectedTask.Notes = newNotes;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task notes updated successfully.");
                Console.ResetColor();
            }

            else if (options == "10")  // Search Filtering Task
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                if (itemCol.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No Task Available!");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    continue; 
                }            

                Console.WriteLine("Input a keyword or category to search for tasks:");
                string searchInput = Console.ReadLine().ToLower();

                var filteredTasks = itemCol.Where(task =>
                    task.Name.ToLower().Contains(searchInput) ||
                    task.Category.ToLower().Contains(searchInput)
                ).ToList();

                if (filteredTasks.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No matching tasks found.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    int count = 1;

                    foreach (var task in filteredTasks)
                    {
                        string completionStatus = task.IsCompleted ? "Completed" : "Not Completed"; 
                        Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Due Date: {task.DueDate}, Category: {task.Category})");
                        Console.ResetColor();
                        Console.WriteLine($"Notes: {task.Notes}"); // Output task notes
                        count++;
                    }
                }
            }

            else if (options == "11")  // Archived Task
            {
                Console.Clear();
                DisplayTaskCount(itemCol);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("\nSelect an option for archiving:");
                Console.WriteLine("[1] - Add Task to Archive");
                Console.WriteLine("[2] - View Tasks in Archive (with password/code)");
                Console.WriteLine("[3] - Return to Main Menu\n");

                string archiveOption = Console.ReadLine();

                switch (archiveOption)
                {
                    case "1":
                        AddTaskToArchive(itemCol, archivedTasks);
                        break;
                    case "2":
                        ViewArchiveWithPassword(archivedTasks);
                        break;
                    case "3":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please select a valid option.");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;
                }
            }
        // Console.ReadKey();
    }

        static void AddNewTask(List<TaskItem> itemCol)
        {
            Console.Write("Input New Task Name: ");
            string newTaskName = Console.ReadLine();

            Console.Write("Input Priority Level (1 - High, 2 - Mid, 3 - Low): ");
            int newTaskPriority;
            while (!int.TryParse(Console.ReadLine(), out newTaskPriority) || newTaskPriority < 1 || newTaskPriority > 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Please enter a valid priority level (1 - High, 2 - Mid, 3 - Low): ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }

            Console.Write("Do you want to set a due date for this task? (Y/N): ");
            string setDueDateOption = Console.ReadLine().ToLower();

            DateTime newTaskDueDate = DateTime.MinValue;

            while (setDueDateOption != "y" && setDueDateOption != "n")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input. Please type (Y/N): ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                setDueDateOption = Console.ReadLine().ToLower();
            }

            if (setDueDateOption == "y")
            {
                Console.Write("Input Due Date for the Task (YYYY-MM-DD): ");
                while (!DateTime.TryParse(Console.ReadLine(), out newTaskDueDate))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid due date (YYYY-MM-DD): ");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            }

            Console.Write("Input Category for the Task: ");
            string newTaskCategory = Console.ReadLine();

            Console.Write("Enter estimated time to complete (HH:MM): ");
            TimeSpan estimatedTime;
            while (!TimeSpan.TryParse(Console.ReadLine(), out estimatedTime))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid time format. Enter estimated time to complete (HH:MM): ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }

            Console.WriteLine("Input Notes for the Task: ");
            string newTaskNotes = Console.ReadLine();

            TaskItem newTask = new TaskItem(newTaskName, newTaskPriority, false, newTaskDueDate, newTaskCategory, estimatedTime, newTaskNotes, false);
            itemCol.Add(newTask);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nTask successfully added.");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }

        static void DisplayTaskCount(List<TaskItem> tasks)
        {
            int taskCount = tasks.Count;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total No. Tasks: {taskCount}\n");
            Console.ResetColor();
        }

        static void AddTaskToArchive(List<TaskItem> tasks, List<TaskItem> archivedTasks)
        {
            Console.Clear();
            DisplayTaskCount(tasks);

            if (tasks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No Task Available!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Current Task TODO:");

            int count = 1;
            foreach (var task in tasks)
            {
                string completionStatus = task.IsCompleted ? "Completed" : "Not Completed";
                Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"\t[{count}] - {task.Name} (Priority: {task.Priority}, Status: {completionStatus}, Due Date: {task.DueDate}, Category: {task.Category})");
                Console.ResetColor();
                count++;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\nEnter the task number to add to archive:");

            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count)
            {
                TaskItem taskToArchive = tasks[taskNumber - 1];
                tasks.RemoveAt(taskNumber - 1); // Remove task from current list
                taskToArchive.IsCompleted = true; // Mark as completed before archiving
                archivedTasks.Add(taskToArchive); // Add to archive list
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task added to archive.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid task number. Please try again.");
                Console.ResetColor();
            }
        }

        static void ViewArchiveWithPassword(List<TaskItem> archivedTasks)
        {
            Console.WriteLine("\nEnter the password to access the archive:");
            string password = Console.ReadLine();

            if (archivedTasks.Count != 0)
            {
                if (password == "admin") // Fixed password check
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\nCurrent tasks in archive:");
                    Console.ResetColor();

                    int count = 1;
                    foreach (var task in archivedTasks)
                    {
                        string completionStatus = task.IsCompleted ? "Completed" : "Not Completed";
                        Console.ForegroundColor = task.IsCompleted ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine($"\t[{count}] {task.Name}: Priority: {task.Priority}, Status: {completionStatus}, Due Date: {task.DueDate}, Category: {task.Category}");
                        Console.ResetColor();
                        count++;
                    }
                    
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nWould you like to remove a task from the archive? (Y/N)");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;

                    string remove = Console.ReadLine().ToLower();

                    if (remove == "y")
                    {
                        RemoveTaskFromArchive(archivedTasks);
                    }
                    else if (remove == "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Task removal canceled.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Task removal canceled.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Access denied. Incorrect password.");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo task available.");
                Console.ResetColor();
            } 
        }

        static void RemoveTaskFromArchive(List<TaskItem> archivedTasks)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the index of the task to remove from the archive:");
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            if (int.TryParse(Console.ReadLine(), out int taskIndex) && taskIndex >= 1 && taskIndex <= archivedTasks.Count)
            {
                TaskItem taskToRemove = archivedTasks[taskIndex - 1];
                archivedTasks.RemoveAt(taskIndex - 1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Task removed from the archive.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid index. Task removal canceled.");
                Console.ResetColor();
            }
        }



        // void LoadTasksFromFile()
        // {
        //     if (File.Exists(filePath))
        //     {
        //         string[] lines = File.ReadAllLines(filePath);
        //         foreach (string line in lines)
        //         {
        //             // Parse each line and create TaskItem objects
        //             string[] data = line.Split(',');
        //             // Assuming the format is: Name, Priority, Category, DueDate, Notes, IsArchived
        //             if (data.Length == 6)
        //             {
        //                 string name = data[0].Trim();
        //                 int priority = int.Parse(data[1].Trim());
        //                 string category = data[2].Trim();
        //                 string notes = data[4].Trim();
        //                 DateTime dueDate = DateTime.Parse(data[3].Trim());
        //                 bool isArchived = bool.Parse(data[5].Trim());

        //                 TaskItem task = new TaskItem(name, priority, false, dueDate, category, TimeSpan.Zero, notes, isArchived);
        //                 itemCol.Add(task);
        //             }
        //         }
        //     }
        // }


        // void SaveTasksToFile()
        // {
        //     List<string> lines = new List<string>();
        //     foreach (var task in itemCol)
        //     {
        //         // Format each TaskItem into a descriptive string
        //         string formattedTask = $"TaskName: {task.Name}, Category: {task.Category}, Priority: {task.Priority}, Due Date: {task.DueDate}, Note: {task.Notes}";
        //         lines.Add(formattedTask);
        //     }
        //     File.WriteAllLines(filePath, lines);
        // }
    }
}


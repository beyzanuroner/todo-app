using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class TodoItem
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

class Program
{
    static string filePath = "todos.json";

    static void Main(string[] args)
    {
        List<TodoItem> todos = LoadTodos();
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== TO-DO APP ===");
            Console.WriteLine("1 - Listeyi Görüntüle");
            Console.WriteLine("2 - Yeni Görev Ekle");
            Console.WriteLine("3 - Görev Sil");
            Console.WriteLine("4 - Görev Tamamla / Geri Al");
            Console.WriteLine("5 - Çıkış");
            Console.Write("Seçimin: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowTodos(todos);
                    break;
                case "2":
                    AddTodo(todos);
                    SaveTodos(todos);
                    break;
                case "3":
                    RemoveTodo(todos);
                    SaveTodos(todos);
                    break;
                case "4":
                    ToggleTodo(todos);
                    SaveTodos(todos);
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Geçersiz seçim.");
                    Pause();
                    break;
            }
        }
    }

    static void ShowTodos(List<TodoItem> todos)
    {
        Console.Clear();
        Console.WriteLine("=== GÖREVLER ===");

        if (todos.Count == 0)
        {
            Console.WriteLine("Henüz görev yok.");
        }
        else
        {
            for (int i = 0; i < todos.Count; i++)
            {
                string status = todos[i].IsCompleted ? "[X]" : "[ ]";
                Console.WriteLine($"{i + 1}. {status} {todos[i].Title}");
            }
        }

        Pause();
    }

    static void AddTodo(List<TodoItem> todos)
    {
        Console.Clear();
        Console.Write("Yeni görev gir: ");
        string title = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(title))
        {
            todos.Add(new TodoItem
            {
                Title = title,
                IsCompleted = false
            });

            Console.WriteLine("Görev eklendi.");
        }
        else
        {
            Console.WriteLine("Boş görev eklenemez.");
        }

        Pause();
    }

    static void RemoveTodo(List<TodoItem> todos)
    {
        Console.Clear();
        ShowTodosShort(todos);
        Console.Write("Silinecek görev numarası: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) &&  index > 0 && index <= todos.Count)
        {
            todos.RemoveAt(index - 1);
            Console.WriteLine("Görev silindi.");
        }
        else
        {
            Console.WriteLine("Geçersiz numara.");
        }

        Pause();
    }

    static void ToggleTodo(List<TodoItem> todos)
    {
        Console.Clear();
        ShowTodosShort(todos);
        Console.Write("Tamamlanacak/geri alınacak görev numarası: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) &&
            index > 0 && index <= todos.Count)
        {
            todos[index - 1].IsCompleted = !todos[index - 1].IsCompleted;
            Console.WriteLine("Görev durumu güncellendi.");
        }
        else
        {
            Console.WriteLine("Geçersiz numara.");
        }

        Pause();
    }

    static void ShowTodosShort(List<TodoItem> todos)
    {
        for (int i = 0; i < todos.Count; i++)
        {
            string status = todos[i].IsCompleted ? "[X]" : "[ ]";
            Console.WriteLine($"{i + 1}. {status} {todos[i].Title}");
        }
    }

    static void SaveTodos(List<TodoItem> todos)
    {
        string json = JsonSerializer.Serialize(todos, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }

    static List<TodoItem> LoadTodos()
    {
        if (!File.Exists(filePath))
            return new List<TodoItem>();

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<TodoItem>>(json);
    }

    static void Pause()
    {
        Console.WriteLine("\nDevam etmek için bir tuşa bas...");
        Console.ReadKey();
    }
}

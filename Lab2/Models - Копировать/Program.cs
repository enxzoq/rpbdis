using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Lab2
{
    internal class Program
    {
        private static IEnumerable<object>? averageSubscriptionFee;

        static void Print<T>(string message, IEnumerable<T>? items)
        {
            Console.WriteLine(message);
            Console.WriteLine("Записи: ");
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    Console.WriteLine(item!.ToString());
                }
            }
            else
            {
                Console.WriteLine("Пусто");
            }
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу");
            Console.ReadKey();
        }

        static void Select(TelecomContext db)
        {
            // 1. Выборка всех данных из таблицы на стороне отношения «один»
            var employees = db.Employees
                .Select(e => new
                {
                    e.FullName,
                    e.Position
                })
                .ToList();
            Print("1. Все сотрудники:", employees);

            // 2. Выборка данных с фильтрацией
            var filteredEmployees = db.Employees
                .Where(e => e.Position == "Менеджер")
                .Select(e => new
                {
                    e.FullName,
                    e.Position
                })
                .ToList();
            Print("2. Отфильтрованные сотрудники (Менеджеры):", filteredEmployees);

            // 3. Выборка данных, сгруппированных по полю с итоговым результатом
            try
            {
                var averageSubscriptionFee = db.ServiceContracts
                    .GroupBy(sc => sc.TariffPlanName)
                    .Select(g => new
                    {
                        TariffPlan = g.Key,
                        AverageFee = g.Average(sc => sc.SubscriptionFee ?? 0)
                    })
                    .ToList();

                Print("Средняя стоимость подписки по тарифным планам:", averageSubscriptionFee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Print("3. Средняя стоимость подписки по тарифным планам:", averageSubscriptionFee);

            // 4. Выборка данных из двух полей двух таблиц, связанных «один-ко-многим»
            var subscriberContracts = db.Subscribers
                .Select(s => new
                {
                    s.FullName,
                    Contracts = s.ServiceContracts.Select(sc => sc.ContractId)
                })
                .ToList();
            Print("4. Контракты подписчиков:", subscriberContracts);

            // 5. Выборка данных из двух таблиц с фильтрацией
            var filteredContracts = db.Subscribers
                .Where(s => s.HomeAddress.Contains("Нью-Йорк"))
                .Select(s => new
                {
                    s.FullName,
                    Contracts = s.ServiceContracts.Select(sc => sc.ContractId)
                })
                .ToList();
            Print("5. Подписчики из Нью-Йорка с контрактами:", filteredContracts);
        }

        static void Insert(TelecomContext db)
        {
            // Вставка нового сотрудника
            var newEmployee = new Employee
            {
                FullName = "Иван Иванов",
                Position = "Менеджер",
                Education = "Высшее"
            };
            db.Employees.Add(newEmployee);
            db.SaveChanges();
            Print("Добавлен новый сотрудник:", new[] { newEmployee });

            // Вставка нового подписчика
            var newSubscriber = new Subscriber
            {
                FullName = "Петр Петров",
                HomeAddress = "Москва",
                PassportData = "1234 567890"
            };
            db.Subscribers.Add(newSubscriber);
            db.SaveChanges();
            Print("Добавлен новый подписчик:", new[] { newSubscriber });
        }

        static void Delete(TelecomContext db)
        {
            // Удаление сотрудника
            var employeeToRemove = db.Employees.FirstOrDefault(e => e.FullName == "Иван Иванов");
            if (employeeToRemove != null)
            {
                db.Employees.Remove(employeeToRemove);
                db.SaveChanges();
                Print($"Удален сотрудник: {employeeToRemove.FullName}", new List<Employee> { employeeToRemove });
            }
            else
            {
                Print("Сотрудник не найден.", new List<Employee>());
            }

            // Удаление подписчика
            var subscriberToRemove = db.Subscribers.FirstOrDefault(s => s.FullName == "Петр Петров");
            if (subscriberToRemove != null)
            {
                db.Subscribers.Remove(subscriberToRemove);
                db.SaveChanges();
                Print($"Удален подписчик: {subscriberToRemove.FullName}", new List<Subscriber> { subscriberToRemove });
            }
            else
            {
                Print("Подписчик не найден.", new List<Subscriber>());
            }
        }

        static void Update(TelecomContext db)
        {
            // Обновление должности сотрудников
            var employeesToUpdate = db.Employees
                .Where(e => e.Position == "Менеджер")
                .ToList();

            foreach (var employee in employeesToUpdate)
            {
                employee.Position = "Старший менеджер";
            }
            db.SaveChanges();
            Print("Обновлены позиции сотрудников:", employeesToUpdate);
        }

        static void Main(string[] args)
        {
            using (var db = new TelecomContext())
            {
                Console.WriteLine("Будет выполнена выборка данных (нажмите любую клавишу) ============");
                Console.ReadKey();
                Select(db);

                Console.WriteLine("Будет выполнена вставка данных (нажмите любую клавишу) ============");
                Console.ReadKey();
                Insert(db);

                Console.WriteLine("Будет выполнено удаление данных (нажмите любую клавишу) ============");
                Console.ReadKey();
                Delete(db);

                Console.WriteLine("Будет выполнено обновление данных (нажмите любую клавишу) ============");
                Console.ReadKey();
                Update(db);
            }
        }
    }
}
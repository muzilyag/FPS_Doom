using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ReportGenerator
{
    public class PlayerData
    {
        public string accountId { get; set; }
        public int maxWavesSurvived { get; set; }
        public int zombieKilled { get; set; }
        public float maxLiveTime { get; set; }
    }
    public class RootData
    {
        public List<PlayerData> players { get; set; }
        public static RootData LoadData(string jsonPath)
        {
            string json = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<RootData>(json);
        }
    }

    public static class Report
    {
        static Report()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static void GenerateReport(string jsonPath, string outputPath, string currentAccountId)
        {
            var data = RootData.LoadData(jsonPath);
            var currentPlayer = data.players.Find(p => p.accountId == currentAccountId);

            if (currentPlayer == null)
                throw new ArgumentException("Пользователь не найден!");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.Content()
                        .Padding(20)
                        .AlignCenter() // Выравнивание всей колонки по центру
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter() // Центрирование элемента внутри колонки
                                .Text(text =>
                                {
                                    text.Span($"Отчет для: {currentPlayer.accountId}").Bold().FontSize(16);
                                    text.EmptyLine();
                                    text.Line($"Волн пережито: {currentPlayer.maxWavesSurvived}");
                                    text.Line($"Убито зомби: {currentPlayer.zombieKilled}");
                                    text.Line($"Макс. время выживания: {currentPlayer.maxLiveTime} сек");
                                });
                        });
                });
            }).GeneratePdf(outputPath);
        }
    }
}
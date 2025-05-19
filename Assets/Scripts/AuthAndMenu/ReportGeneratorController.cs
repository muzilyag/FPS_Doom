using UnityEngine;
using System.IO;
using ReportGenerator;
using UnityEngine.UI;
using QuestPDF.Infrastructure;
public class ReportGeneratorController : MonoBehaviour
{
    public void GenerateReport()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        // Путь к users.json
        string jsonPath = Path.Combine(Application.persistentDataPath, "users.json");

        // Путь для сохранения отчета
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "report.pdf");
        string currentAccountId = PlayerPrefs.GetString("CurrentAccountId");

        try
        {
            // Вызов метода генерации с тремя параметрами
            Report.GenerateReport(jsonPath, outputPath, currentAccountId);
            Debug.Log("Отчет создан: " + outputPath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Ошибка: " + ex.Message);
        }
    }
}
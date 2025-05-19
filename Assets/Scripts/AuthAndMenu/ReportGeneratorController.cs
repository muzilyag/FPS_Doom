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
        // ���� � users.json
        string jsonPath = Path.Combine(Application.persistentDataPath, "users.json");

        // ���� ��� ���������� ������
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "report.pdf");
        string currentAccountId = PlayerPrefs.GetString("CurrentAccountId");

        try
        {
            // ����� ������ ��������� � ����� �����������
            Report.GenerateReport(jsonPath, outputPath, currentAccountId);
            Debug.Log("����� ������: " + outputPath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("������: " + ex.Message);
        }
    }
}
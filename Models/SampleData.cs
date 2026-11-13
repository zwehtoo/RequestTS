using Microsoft.AspNetCore.Mvc;

namespace RequestTS.Models
{
    public static class SampleData
    {
        public static List<RequestItem> Requests { get; } =
            new List<RequestItem>
            {
                new RequestItem
                {
                    Id = 1,
                    RequestNo = "REQ-1001",
                    Project = "TradeNet Portal",
                    Title = "Export licence စာရင်းတွင် Filter ထည့်ရန်",
                    Description = "လိုအပ်ချက်ကို စစ်ဆေးပြီး လုပ်ဆောင်ပေးရန်။",
                    Priority = "Normal",
                    Requester = "May Thu",
                    Assignee = "Aung Min",
                    Status = RequestStatus.InProgress,
                    CreatedAt = new DateTime(2026, 11, 3, 9, 10, 0),
                    AssignedAt = new DateTime(2026, 11, 6, 11, 0, 0),
                    DueDate = new DateOnly(2026, 11, 12),

                    Activities = new List<RequestActivity>
                    {
                        new()
                        {
                            Actor = "May Thu",
                            Action = "created this request",
                            CreatedAt = new DateTime(2026, 11, 3, 9, 10, 0)
                        },
                        new()
                        {
                            Actor = "May Thu",
                            Action = "assigned to Aung Min",
                            CreatedAt = new DateTime(2026, 11, 3, 10, 15, 0)
                        },
                        new()
                        {
                            Actor = "Aung Min",
                            Action = "changed status to In Progress",
                            CreatedAt = new DateTime(2026, 11, 3, 11, 20, 0)
                        },
                        new()
                        {
                            Actor = "May Thu",
                            Action = "reassigned to Hnin Ei",
                            Note = "Report logic review",
                            CreatedAt = new DateTime(2026, 11, 6, 11, 0, 0)
                        }
                    }
                },

                new RequestItem
                {
                    Id = 2,
                    RequestNo = "REQ-1002",
                    Project = "TradeNet Portal",
                    Title = "Monthly summary report ပြင်ဆင်ရန်",
                    Description = "Monthly report ကို ပြင်ဆင်ရန်။",
                    Priority = "High",
                    Requester = "May Thu",
                    Assignee = "Hnin Ei",
                    Status = RequestStatus.Completed,
                    CreatedAt = new DateTime(2026, 11, 2, 10, 20, 0),
                    CompletedAt = new DateTime(2026, 11, 5, 16, 40, 0),
                    DueDate = new DateOnly(2026, 11, 5)
                },

                new RequestItem
                {
                    Id = 3,
                    RequestNo = "REQ-1003",
                    Project = "Mobile App",
                    Title = "Notification setting ထည့်ရန်",
                    Description = "Notification setting feature ထည့်ပေးရန်။",
                    Priority = "Normal",
                    Requester = "May Thu",
                    Assignee = null,
                    Status = RequestStatus.Submitted,
                    CreatedAt = new DateTime(2026, 11, 6, 8, 50, 0)
                },

                new RequestItem
                {
                    Id = 4,
                    RequestNo = "REQ-1004",
                    Project = "Internal Operations",
                    Title = "Access permission စစ်ဆေးရန်",
                    Description = "Access permission ကို စစ်ဆေးရန်။",
                    Priority = "Low",
                    Requester = "May Thu",
                    Assignee = "Ko Zaw",
                    Status = RequestStatus.OnHold,
                    CreatedAt = new DateTime(2026, 11, 4, 14, 0, 0)
                },

                new RequestItem
                {
                    Id = 5,
                    RequestNo = "REQ-1005",
                    Project = "Mobile App",
                    Title = "Login error ပြင်ဆင်ရန်",
                    Description = "Login error ကို စစ်ဆေးပြင်ဆင်ရန်။",
                    Priority = "High",
                    Requester = "May Thu",
                    Assignee = "Hnin Ei",
                    Status = RequestStatus.Assigned,
                    CreatedAt = new DateTime(2026, 11, 5, 9, 0, 0)
                }
            };

        public static string[] Developers =
        {
            "Aung Min",
            "Hnin Ei",
            "Ko Zaw",
            "Mg Mg"
        };
    }
}

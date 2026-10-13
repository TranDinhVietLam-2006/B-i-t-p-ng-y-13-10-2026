
using System.ComponentModel;

namespace Bài46
{
    public class Student : INotifyPropertyChanged
    {
        private string studentId = "";
        private string fullName = "";
        private string age = "";
        private string gender = "Nam";
        private string avatarPath = "";

        public string StudentId
        {
            get { return studentId; }
            set
            {
                if (studentId != value)
                {
                    studentId = value;
                    OnPropertyChanged(nameof(StudentId));
                }
            }
        }

        public string FullName
        {
            get { return fullName; }
            set
            {
                if (fullName != value)
                {
                    fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string Age
        {
            get { return age; }
            set
            {
                if (age != value)
                {
                    age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        public string AvatarPath
        {
            get { return avatarPath; }
            set
            {
                if (avatarPath != value)
                {
                    avatarPath = value;
                    OnPropertyChanged(nameof(AvatarPath));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}

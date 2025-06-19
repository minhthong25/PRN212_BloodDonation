using System;
using System.Windows;
using Repository.Models;

namespace BloodBankSystem.AdminDisplay
{
    /// <summary>
    /// Interaction logic for EditBloodEvent.xaml
    /// </summary>
    public partial class EditBloodEvent : Window
    {
        private readonly BloodEvent _bloodEvent;
        public EditBloodEvent()
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SecureEVotingSystem {
    class ApplicationViewModel : INotifyPropertyChanged {
        private string isHidenAutorization;

        public string IsHidenAutorization {
            get { return isHidenAutorization; }
            set { 
                isHidenAutorization = value;
                OnPropertyChanged("IsHidenAutorization");
            }
        }

        private string isHidenVoting;

        public string IsHidenVoting {
            get { return isHidenVoting; }
            set {
                isHidenVoting = value;
                OnPropertyChanged("IsHidenVoting");
            }
        }

        private string isHidenTelling;

        public string IsHidenTelling {
            get { return isHidenTelling; }
            set {
                isHidenTelling = value;
                OnPropertyChanged("IsHidenTelling");
            }
        }

        private enum WindowType { 
            Autorization,
            Voting,
            Telling
        }

        private WindowType typeOfWindow;

        private WindowType TypeOfWindow {
            get { return typeOfWindow; }
            set {
                IsHidenVoting = "Hidden";
                IsHidenAutorization = "Hidden";
                IsHidenTelling = "Hidden";
                if (value == WindowType.Voting)
                    IsHidenVoting = "Visible";
                else if (value == WindowType.Autorization)
                    IsHidenAutorization = "Visible";
                else IsHidenTelling = "Visible";
                typeOfWindow = value;
            } 
        }

        private string validatorLog;
        public string ValidatorLog {
            get => validatorLog;
            set{
                validatorLog = value;
                OnPropertyChanged("Validator");
            }
        }
        private string agencyLog;
        public string AgencyLog {
            get => agencyLog;
            set {
                agencyLog = value;
                OnPropertyChanged("Agency");
            }
        }
        
        private string elector;
        public string Elector {
            get => elector;
            set {
                elector = value;
                OnPropertyChanged("elector");
            }
        }

        private string name;

        public string Name {
            get { return name; }
            set { 
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string password;

        public string Password {
            get { return password; }
            set { 
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private int result1;

        public int Result1 {
            get { return result1; }
            set { 
                result1 = value;
                OnPropertyChanged();
            }
        }

        private int result2;

        public int Result2 {
            get { return result2; }
            set { 
                result2 = value;
                OnPropertyChanged();
            }
        }


        private Dictionary<Tuple<string, string>, Voter> autorizedVoters = new Dictionary<Tuple<string, string>, Voter>();
        private Voter currentVoter;
        private Agency agency = new Agency();
        private Validator validator = new Validator();

        public ApplicationViewModel() {
            ValidatorLog = "Лог регистратора";
            AgencyLog = "Лог агенства";
            Elector = "Лог избирателя";
            TypeOfWindow = WindowType.Autorization;
            Name = "Введите имя";
            Password = "Введите пароль";
            Result1 = 0;
            Result2 = 0;
        }

        public ICommand OpenVoting {
            get => new DelegateCommand(
                    (obj) => {
                        var autorizedVoterDictionaryKey = Tuple.Create(Name, Password);
                        if (autorizedVoters.ContainsKey(autorizedVoterDictionaryKey)) {
                            currentVoter = autorizedVoters[autorizedVoterDictionaryKey];
                        } else {
                            currentVoter = new Voter(Name, Password, validator.Encryptor);
                            autorizedVoters.Add(autorizedVoterDictionaryKey, currentVoter);
                        }
                        var blindKey = currentVoter.GetSignedKey(validator.Encryptor);
                        var criptedNumber = validator.Encryptor.Crypt(currentVoter.Number);
                        var criptedPassword = validator.Encryptor.Crypt(currentVoter.Password);
                        if (!validator.BlindSigningKey(criptedPassword, criptedNumber, blindKey, out int blindedSignedKey)) {
                            return;
                        }
                        if(!currentVoter.DeblindAndCheckValidatorSign(validator.Encryptor, blindedSignedKey, 
                            out RSACryptor voterEncryptor, out int signedKey))
                            return;
                        if(!agency.CheckValidatorSignKeyVoter(validator.Encryptor, signedKey, voterEncryptor.Key))
                            return;
                        TypeOfWindow = WindowType.Voting;
                    }
                );
        }

        private void VotingOperation() {
            TypeOfWindow = WindowType.Autorization;
            Name = "";
            Password = "";
        }

        public ICommand Voting1 {
            get => new DelegateCommand(
                    (obj) => {
                        VotingOperation();
                        Result1++;
                    }
                );
        }

        public ICommand Voting2 {
            get => new DelegateCommand(
                    (obj) => {
                        VotingOperation();
                        Result2++;
                    }
                );
        }
        public ICommand OpenTelling {
            get => new DelegateCommand(
                    (obj) => {
                        TypeOfWindow = WindowType.Telling;
                    }
                );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

namespace IconRoller

open System
open System.Net
open Suave
open Suave.Filters
open Suave.Operators

open CoreTweet

open IconRoller.App

module Main = 
  begin

    [<EntryPoint>]
    let main argv =
      printfn "starting server..."

      let config = 
        let port = System.Environment.GetEnvironmentVariable("PORT")
        let ip127  = IPAddress.Parse("127.0.0.1")
        let ipZero = IPAddress.Parse("0.0.0.0")
        { 
          defaultConfig with 
            bindings=
            [ 
              (
                if port = null then // localhost
                  HttpBinding.create HTTP ip127 (uint16 8080)
                else // Heroku
                  HttpBinding.create HTTP ipZero (uint16 port)
              ) 
            ] 
        }

      let webPart =
        choose [
          path "/twitter_login" >=>
            Twit.login

          path "/twitter_redirect" >=>
            Twit.redirect
          
          path "/logout" >=>
            App.clearSession >=> App.returnToHome

          path "/" >=>
            App.session (fun session ->
              let state = 
                match session with
                  | AuthorizeSession os ->
                    sprintf "AuthorizeSession: %A" os.AuthorizeUri
                  | TokensSession t ->
                    t.Account.VerifyCredentials().ScreenName
                    |> sprintf "TokensSession: %s"
                  | _ -> 
                    "None"
              in

              View.info state |> Successful.OK
            )
        ]

      startWebServer config webPart

      printfn "exiting server..."
      0

  end
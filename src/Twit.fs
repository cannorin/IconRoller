namespace IconRoller

open System
open CoreTweet

open Suave
open Suave.Operators
open Suave.Authentication

open IconRoller.App

module Twit = 
  begin

    let login =
      authenticated Cookie.CookieLife.Session false 
      >=> App.session (
        fun session ->
          match session with
            | AuthorizeSession os ->
              App.clearSession
              >=> Redirection.redirect "/twitter_login"
            | TokensSession _ ->
              App.returnToHome
            | NoSession ->
              let oas = OAuth.Authorize(Property.ConsumerKey(), Property.ConsumerSecret(), oauthCallback=Property.TwitterRedirectPath()) in
              App.sessionSet [
                ("r_token", oas.RequestToken)
                ("r_secret", oas.RequestTokenSecret)
              ]
              >=> Redirection.redirect (oas.AuthorizeUri.ToString()) 
        )

    let redirect =
      App.session (fun session ->
        match session with
          | AuthorizeSession os ->
            let browse =
              request (
                fun r ->
                  match r.queryParam "oauth_verifier" with
                    | Choice1Of2 pin -> 
                      let tokens = os.GetTokens(pin) in
                      App.sessionSet [
                        ("a_key", tokens.AccessToken)
                        ("a_secret", tokens.AccessTokenSecret)
                      ]
                      >=> App.returnToHome
                    | Choice2Of2 msg -> 
                      App.clearSession
                      >=> App.returnToHome
              )
            in browse
          | _ -> 
            App.returnToHome
      )

  end